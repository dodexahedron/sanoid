// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

using System.Collections.Concurrent;
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
    public DateTimeOffset LastDailySnapshot => Properties.TryGetValue( "sanoid.net:lastdailysnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastFrequentSnapshot => Properties.TryGetValue( "sanoid.net:lastfrequentsnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastHourlySnapshot => Properties.TryGetValue( "sanoid.net:lasthourlysnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastMonthlySnapshot => Properties.TryGetValue( "sanoid.net:lastmonthlysnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastWeeklySnapshot => Properties.TryGetValue( "sanoid.net:lastweeklysnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public DateTimeOffset LastYearlySnapshot => Properties.TryGetValue( "sanoid.net:lastyearlysnapshot", out ZfsProperty? prop ) && DateTimeOffset.TryParse( prop.Value, out DateTimeOffset timestamp ) ? timestamp : DateTimeOffset.UnixEpoch;

    [JsonIgnore]
    public bool PruneSnapshots
    {
        get
        {
            string valueString = Properties.TryGetValue( "sanoid.net:prunesnapshots", out ZfsProperty? prop ) ? prop.Value : "false";
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

    /// <inheritdoc />
    public override string ToString( )
    {
        return JsonSerializer.Serialize( this );
    }

    public void AddSnapshot( Snapshot snap )
    {
        Logger.Debug( "Adding snapshot {0} to dataset {1}", snap.Name, Name );
        Snapshots[ snap.Name ] = snap;
    }
}
