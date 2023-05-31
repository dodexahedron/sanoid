﻿// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

using System.Text.Json;
using Sanoid.Interop.Concurrency;
using Sanoid.Interop.Libc.Enums;
using Sanoid.Interop.Zfs.ZfsCommandRunner;
using Sanoid.Interop.Zfs.ZfsTypes;
using Sanoid.Settings.Settings;

namespace Sanoid;

internal static class SnapshotTasks
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger( );

    /// <exception cref="InvalidOperationException">If an invalid value is returned when getting the mutex</exception>
    internal static Errno TakeAllConfiguredSnapshots( IZfsCommandRunner commandRunner, SanoidSettings settings, SnapshotPeriod period, DateTimeOffset timestamp, ref Dictionary<string, Dataset> datasets )
    {
        const string snapshotMutexName = "Global\\Sanoid.net_Snapshots";
        using MutexAcquisitionResult mutexAcquisitionResult = Mutexes.GetAndWaitMutex( snapshotMutexName );
        switch ( mutexAcquisitionResult.ErrorCode )
        {
            case MutexAcquisitionErrno.Success:
            {
                Logger.Trace( "Successfully acquired mutex {0}", snapshotMutexName );
            }
                break;
            // All of the error cases can just fall through, here, because we really don't care WHY it failed,
            // for the purposes of taking snapshots. We'll just let the user know and then not take snapshots.
            case MutexAcquisitionErrno.InProgess:
            case MutexAcquisitionErrno.IoException:
            case MutexAcquisitionErrno.AbandonedMutex:
            case MutexAcquisitionErrno.WaitHandleCannotBeOpened:
            case MutexAcquisitionErrno.PossiblyNullMutex:
            case MutexAcquisitionErrno.AnotherProcessIsBusy:
            case MutexAcquisitionErrno.InvalidMutexNameRequested:
            {
                Logger.Error( mutexAcquisitionResult.Exception, "Failed to acquire mutex {0}. Error code {1}", snapshotMutexName, mutexAcquisitionResult.ErrorCode );
                return mutexAcquisitionResult;
            }
            default:
                throw new InvalidOperationException( "An invalid value was returned from GetMutex", mutexAcquisitionResult.Exception );
        }

        Logger.Info( "Begin taking snapshots for all configured datasets" );

        foreach ( ( string _, Dataset ds ) in datasets )
        {
            if ( !settings.Templates.TryGetValue( ds.Template, out TemplateSettings? template ) )
            {
                string errorMessage = $"Template {ds.Template} specified for {ds.Name} not found in configuration - skipping";
                Logger.Error( errorMessage );
                continue;
            }

            // The MaxBy function will fail if the sort key is a value type (it is - DateTimeOffset) and the collection is null
            // ReSharper disable SimplifyLinqExpressionUseMinByAndMaxBy
            List<ZfsProperty> propsToSet = new( );
            if ( ds is { TakeSnapshots: true, Enabled: true } )
            {
                if ( ds.IsFrequentSnapshotNeeded( template, timestamp ) )
                {
                    bool frequentSnapshotTaken = TakeSnapshot( commandRunner, settings, ds, SnapshotPeriod.Frequent, timestamp, out Snapshot? snapshot );
                    if ( frequentSnapshotTaken && ds.Properties.TryGetValue( ZfsProperty.DatasetLastFrequentSnapshotTimestampPropertyName, out ZfsProperty? prop ) )
                    {
                        Logger.Trace( "Frequent snapshot {0} taken successfully", snapshot?.Name ?? $"of {ds.Name}" );

                        prop.Value = timestamp.ToString( "O" );
                        prop.PropertySource = ZfsPropertySource.Local;
                        ds[ ZfsProperty.DatasetLastFrequentSnapshotTimestampPropertyName ] = prop;
                        propsToSet.Add( prop );
                    }
                }

                if ( ds.IsHourlySnapshotNeeded( template.SnapshotRetention, timestamp ) )
                {
                    bool hourlySnapshotTaken = TakeSnapshot( commandRunner, settings, ds, SnapshotPeriod.Hourly, timestamp, out Snapshot? snapshot );
                    if ( hourlySnapshotTaken && ds.Properties.TryGetValue( ZfsProperty.DatasetLastHourlySnapshotTimestampPropertyName, out ZfsProperty? prop ) )
                    {
                        Logger.Trace( "Hourly snapshot {0} taken successfully", snapshot?.Name ?? $"of {ds.Name}" );

                        prop.Value = timestamp.ToString( "O" );
                        prop.PropertySource = ZfsPropertySource.Local;
                        ds[ ZfsProperty.DatasetLastHourlySnapshotTimestampPropertyName ] = prop;
                        propsToSet.Add( prop );
                    }
                }

                if ( ds.IsDailySnapshotNeeded( template.SnapshotRetention, timestamp ) )
                {
                    bool dailySnapshotTaken = TakeSnapshot( commandRunner, settings, ds, SnapshotPeriod.Daily, timestamp, out Snapshot? snapshot );
                    if ( dailySnapshotTaken && ds.Properties.TryGetValue( ZfsProperty.DatasetLastDailySnapshotTimestampPropertyName, out ZfsProperty? prop ) )
                    {
                        Logger.Trace( "Daily snapshot {0} taken successfully", snapshot?.Name ?? $"of {ds.Name}" );
                        prop.Value = timestamp.ToString( "O" );
                        prop.PropertySource = ZfsPropertySource.Local;
                        ds[ ZfsProperty.DatasetLastDailySnapshotTimestampPropertyName ] = prop;
                        propsToSet.Add( prop );
                    }
                }

                if ( ds.IsWeeklySnapshotNeeded( template, timestamp ) )
                {
                    bool weeklySnapshotTaken = TakeSnapshot( commandRunner, settings, ds, SnapshotPeriod.Weekly, timestamp, out Snapshot? snapshot );
                    if ( weeklySnapshotTaken && ds.Properties.TryGetValue( ZfsProperty.DatasetLastWeeklySnapshotTimestampPropertyName, out ZfsProperty? prop ) )
                    {
                        Logger.Trace( "Weekly snapshot {0} taken successfully", snapshot?.Name ?? $"of {ds.Name}" );
                        prop.Value = timestamp.ToString( "O" );
                        prop.PropertySource = ZfsPropertySource.Local;
                        ds[ ZfsProperty.DatasetLastWeeklySnapshotTimestampPropertyName ] = prop;
                        propsToSet.Add( prop );
                    }
                }

                commandRunner.SetZfsProperties( ds.Name, propsToSet.ToArray( ) );
            }
        }

        Logger.Debug( "Finished taking snapshots" );

        // snapshotName is a defined string. Thus, this NullReferenceException is not possible.
        // ReSharper disable once ExceptionNotDocumentedOptional
        Mutexes.ReleaseMutex( snapshotMutexName );

        return Errno.EOK;
    }

    internal static bool TakeSnapshot( IZfsCommandRunner commandRunner, SanoidSettings settings, Dataset ds, SnapshotPeriod snapshotPeriod, DateTimeOffset timestamp, out Snapshot? snapshot )
    {
        Logger.Debug( "TakeSnapshot called for {0} with period {1}", ds.Name, snapshotPeriod );
        snapshot = null;
        if ( !ds.Enabled )
        {
            Logger.Trace( "Dataset {0} is not enabled. Skipping", ds.Name );
            return false;
        }

        if ( !ds.TakeSnapshots )
        {
            Logger.Trace( "Dataset {0} is not configured to take snapshots. Skipping", ds.Name );
            return false;
        }

        if ( ds.Recursion == SnapshotRecursionMode.Zfs && ds[ "sanoid.net:recursion" ]?.PropertySource != ZfsPropertySource.Local )
        {
            Logger.Trace( "Ancestor of dataset {0} is configured for zfs native recursion and recursion not set locally. Skipping", ds.Name );
            return false;
        }

        if ( !settings.Templates.TryGetValue( ds.Template, out TemplateSettings? template ) )
        {
            Logger.Error( "Template {0} for dataset {1} not found in configuration. Skipping", ds.Template, ds.Name );
            return false;
        }

        switch ( snapshotPeriod.Kind )
        {
            case SnapshotPeriodKind.Frequent:
                if ( template.SnapshotRetention.Frequent == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            case SnapshotPeriodKind.Hourly:
                if ( template.SnapshotRetention.Hourly == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            case SnapshotPeriodKind.Daily:
                if ( template.SnapshotRetention.Daily == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            case SnapshotPeriodKind.Weekly:
                if ( template.SnapshotRetention.Weekly == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            case SnapshotPeriodKind.Monthly:
                if ( template.SnapshotRetention.Monthly == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            case SnapshotPeriodKind.Yearly:
                if ( template.SnapshotRetention.Yearly == 0 )
                {
                    Logger.Trace( "Requested {0} snapshot, but dataset {1} does not want them. Skipping", snapshotPeriod, ds.Name );
                    return false;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException( nameof( snapshotPeriod ), snapshotPeriod, $"Unexpected value received for snapshotPeriod for dataset {ds.Name}. Snapshot not taken." );
        }

        Logger.Trace( "Dataset {0} will have a snapshot taken with these settings: {1}", ds.Name, JsonSerializer.Serialize( new { ds, template } ) );

        if ( commandRunner.TakeSnapshot( ds, snapshotPeriod, timestamp, settings, out snapshot ) )
        {
            ds.Snapshots[ snapshot.Name ] = snapshot;
            Logger.Info( "Snapshot {0} successfully taken", snapshot.Name );
            return true;
        }

        Logger.Error( "Snapshot for dataset {0} not taken", ds.Name );
        return false;
    }

    internal static void UpdateZfsDatasetSchema( ref Dictionary<string, Dictionary<string, ZfsProperty>> poolPropertyCollections, IZfsCommandRunner zfsCommandRunner )
    {
        Logger.Debug( "Requested update of zfs properties schema" );
        foreach ( ( string poolName, Dictionary<string, ZfsProperty> propertiesToAdd ) in poolPropertyCollections )
        {
            Logger.Info( "Updating properties for pool {0}", poolName );
            ZfsProperty[] propertyArray = propertiesToAdd.Values.ToArray( );
            if ( !zfsCommandRunner.SetZfsProperties( poolName, propertyArray ) )
            {
                Logger.Error( "Failed updating properties for pool {0}. Unset properties: {1}", poolName, JsonSerializer.Serialize( propertyArray ) );
            }

            Logger.Info( "Finished updating properties for pool {0}", poolName );
        }

        Logger.Debug( "Finished updating zfs properties schema for all pool roots" );
    }
}
