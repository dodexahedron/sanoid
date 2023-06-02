// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using NLog;
using Sanoid.Settings.Settings;

namespace Sanoid.Interop.Zfs.ZfsTypes;

/// <summary>
///     A ZFS Dataset object. Can be a filesystem or volume.
/// </summary>
public class Dataset : ZfsObjectBase
{
    /// <summary>
    ///     Creates a new <see cref="Dataset" /> with the specified name and kind, optionally performing name validation
    /// </summary>
    /// <param name="name">The name of the new <see cref="Dataset" /></param>
    /// <param name="kind">The <see cref="DatasetKind" /> of Dataset to create</param>
    /// <param name="validateName">
    ///     Whether to validate the name of the new <see cref="Dataset" /> (<see langword="true" />) or
    ///     not (<see langword="false" /> - default)
    /// </param>
    /// <param name="validatorRegex">The <see cref="Regex" /> to user for name validation</param>
    public Dataset( string name, DatasetKind kind, bool validateName = false, Regex? validatorRegex = null )
        : base( name, (ZfsObjectKind)kind, validatorRegex, validateName )
    {
    }

    [JsonIgnore]
    public bool Enabled
    {
        get
        {
            string valueString = Properties.TryGetValue( "sanoid.net:enabled", out ZfsProperty? prop ) ? prop.Value : "false";
            return bool.TryParse( valueString, out bool result ) && result;
        }
    }

    [JsonIgnore]
    public DateTimeOffset LastDailySnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastDailySnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastFrequentSnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastFrequentSnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastHourlySnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastHourlySnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastMonthlySnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastMonthlySnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastWeeklySnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastWeeklySnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastYearlySnapshotTimestamp => Properties.TryGetValue( ZfsProperty.DatasetLastYearlySnapshotTimestampPropertyName, out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public bool PruneSnapshots
    {
        get
        {
            string valueString = Properties.TryGetValue( ZfsProperty.PruneSnapshotsPropertyName, out ZfsProperty? prop ) ? prop.Value : "false";
            return bool.TryParse( valueString, out bool result ) && result;
        }
    }

    [JsonIgnore]
    public SnapshotRecursionMode Recursion
    {
        get
        {
            string valueString = Properties.TryGetValue( "sanoid.net:recursion", out ZfsProperty? prop ) ? prop.Value : "false";
            return valueString;
        }
    }

    public ConcurrentDictionary<string, Snapshot> Snapshots { get; } = new( );

    [JsonIgnore]
    public bool TakeSnapshots
    {
        get
        {
            string valueString = Properties.TryGetValue( "sanoid.net:takesnapshots", out ZfsProperty? prop ) ? prop.Value : "false";
            return bool.TryParse( valueString, out bool result ) && result;
        }
    }

    [JsonIgnore]
    public string Template => Properties.TryGetValue( "sanoid.net:template", out ZfsProperty? prop ) ? prop.Value : "default";

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger( );

    public bool IsFrequentSnapshotNeeded( TemplateSettings template, DateTimeOffset timestamp )
    {
        //Exit early if retention settings say no frequent
        if ( !template.SnapshotRetention.IsFrequentWanted )
        {
            return false;
        }
        // Yes, this can all be done in-line, but this is easier to debug, is more explicit, and the compiler is
        // going to optimize it all away anyway.
        Logger.Trace( "Checking if frequent snapshot is needed for dataset {0} at timestamp {1:O}", Name, timestamp );
        int currentFrequentPeriodOfHour = template.SnapshotTiming.GetPeriodOfHour( timestamp );
        int lastFrequentSnapshotPeriodOfHour = template.SnapshotTiming.GetPeriodOfHour( LastFrequentSnapshotTimestamp );
        double minutesSinceLastFrequentSnapshot = ( timestamp - LastFrequentSnapshotTimestamp ).TotalMinutes;
        // Check if more than FrequentPeriod ago or if the period of the hour is different.
        bool frequentSnapshotNeeded = minutesSinceLastFrequentSnapshot >= template.SnapshotTiming.FrequentPeriod || lastFrequentSnapshotPeriodOfHour != currentFrequentPeriodOfHour;
        Logger.Debug( "Frequent snapshot is {2}needed for dataset {0} at timestamp {1:O}", Name, timestamp, frequentSnapshotNeeded ? "" : "not " );
        return frequentSnapshotNeeded;
    }

    public bool IsHourlySnapshotNeeded( SnapshotRetentionSettings retention, DateTimeOffset timestamp )
    {
        //Exit early if retention settings say no hourlies
        if ( !retention.IsHourlyWanted )
        {
            return false;
        }
        // Yes, this can all be done in-line, but this is easier to debug, is more explicit, and the compiler is
        // going to optimize it all away anyway.
        Logger.Trace( "Checking if hourly snapshot is needed for dataset {0} at timestamp {1:O}", Name, timestamp );
        TimeSpan timeSinceLastHourlySnapshot = timestamp - LastHourlySnapshotTimestamp;
        bool atLeastOneHourSinceLastHourlySnapshot = timeSinceLastHourlySnapshot.TotalHours >= 1d;
        // Check if more than an hour ago or if hour is different
        bool hourlySnapshotNeeded = atLeastOneHourSinceLastHourlySnapshot
                                    || LastHourlySnapshotTimestamp.LocalDateTime.Hour != timestamp.LocalDateTime.Hour;
        Logger.Debug( "Hourly snapshot is {2}needed for dataset {0} at timestamp {1:O}", Name, timestamp, hourlySnapshotNeeded ? "" : "not " );
        return hourlySnapshotNeeded;
    }

    public bool IsDailySnapshotNeeded( SnapshotRetentionSettings retention, DateTimeOffset timestamp )
    {
        //Exit early if retention settings say no dailies
        if ( !retention.IsDailyWanted )
        {
            return false;
        }
        // Yes, this can all be done in-line, but this is easier to debug, is more explicit, and the compiler is
        // going to optimize it all away anyway.
        Logger.Trace( "Checking if daily snapshot is needed for dataset {0} at timestamp {1:O}", Name, timestamp );
        TimeSpan timeSinceLastDailySnapshot = timestamp - LastDailySnapshotTimestamp;
        bool atLeastOneDaySinceLastDailySnapshot = timeSinceLastDailySnapshot.TotalDays >= 1d;
        // Check if more than a day ago or if a different day of the year
        bool lastDailyOnDifferentDayOfYear = LastDailySnapshotTimestamp.LocalDateTime.DayOfYear != timestamp.LocalDateTime.DayOfYear;
        bool dailySnapshotNeeded = atLeastOneDaySinceLastDailySnapshot || lastDailyOnDifferentDayOfYear;
        Logger.Debug( "Daily snapshot is {2}needed for dataset {0} at timestamp {1:O}", Name, timestamp, dailySnapshotNeeded ? "" : "not " );
        return dailySnapshotNeeded;
    }

    public bool IsWeeklySnapshotNeeded( TemplateSettings template, DateTimeOffset timestamp )
    {
        //Exit early if retention settings say no weeklies
        if ( !template.SnapshotRetention.IsWeeklyWanted )
        {
            return false;
        }
        // Yes, this can all be done in-line, but this is easier to debug, is more explicit, and the compiler is
        // going to optimize it all away anyway.
        Logger.Trace( "Checking if weekly snapshot is needed for dataset {0} at timestamp {1:O}", Name, timestamp );
        TimeSpan timeSinceLastWeeklySnapshot = timestamp - LastWeeklySnapshotTimestamp;
        bool atLeastOneWeekSinceLastWeeklySnapshot = timeSinceLastWeeklySnapshot.TotalDays >= 7d;
        // Check if more than a week ago or if the week number is different by local rules, using the chosen day as the first day of the week
        int lastWeeklySnapshotWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear( LastWeeklySnapshotTimestamp.LocalDateTime, CalendarWeekRule.FirstDay, template.SnapshotTiming.WeeklyDay );
        int currentWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear( timestamp.LocalDateTime, CalendarWeekRule.FirstDay, template.SnapshotTiming.WeeklyDay );
        bool weeklySnapshotNeeded = atLeastOneWeekSinceLastWeeklySnapshot || currentWeekNumber != lastWeeklySnapshotWeekNumber;
        Logger.Debug( "Weekly snapshot is {2}needed for dataset {0} at timestamp {1:O}", Name, timestamp, weeklySnapshotNeeded ? "" : "not " );
        return weeklySnapshotNeeded;
    }

    /// <inheritdoc />
    public override string ToString( )
    {
        return JsonSerializer.Serialize( this );
    }

    public void AddSnapshot( Snapshot snap )
    {
        Logger.Trace( "Adding snapshot {0} to dataset object {1}", snap.Name, Name );
        Snapshots[ snap.Name ] = snap;
    }
}
