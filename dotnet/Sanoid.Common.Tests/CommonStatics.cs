﻿// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

namespace Sanoid.Common.Tests;

internal class CommonStatics
{
    public static readonly Dictionary<string, string?> MockBaseConfigDictionary = new( )
    {
        { "$schema", "Sanoid.schema.json" },
        { "$id", "Sanoid.json" },
        { "TakeSnapshots", "False" },
        { "PruneSnapshots", "False" },
        { "ConfigurationPathBase", "/etc/sanoid" },
        { "CacheDirectory", "/var/cache/sanoid" },
        { "RunDirectory", "/var/run/sanoid" },
        { "DryRun", "False" },
        { "Formatting", null },
        { "Formatting:SnapshotNaming", null },
        { "Formatting:SnapshotNaming:ComponentSeparator", "_" },
        { "Formatting:SnapshotNaming:Prefix", "autosnap" },
        { "Formatting:SnapshotNaming:TimestampFormatString", "yyyy-MM-dd_HH\\:mm\\:ss" },
        { "Formatting:SnapshotNaming:FrequentSuffix", "frequently" },
        { "Formatting:SnapshotNaming:HourlySuffix", "hourly" },
        { "Formatting:SnapshotNaming:DailySuffix", "daily" },
        { "Formatting:SnapshotNaming:WeeklySuffix", "weekly" },
        { "Formatting:SnapshotNaming:MonthlySuffix", "monthly" },
        { "Formatting:SnapshotNaming:YearlySuffix", "yearly" },
        { "PlatformUtilities", null },
        { "PlatformUtilities:ps", "/usr/bin/ps" },
        { "PlatformUtilities:zfs", "/usr/local/sbin/zfs" },
        { "PlatformUtilities:zpool", "/usr/local/sbin/zpool" },
        { "Monitoring", null },
        { "Monitoring:Nagios", null },
        { "Monitoring:Nagios:MonitorType", "Nagios" },
        { "Monitoring:Nagios:Capacity", "False" },
        { "Monitoring:Nagios:Health", "False" },
        { "Monitoring:Nagios:Snapshots", "False" },
        { "Datasets", null },
        { "Templates", null },
        { "Templates:default", null },
        { "Templates:default:AutoSnapshot", "True" },
        { "Templates:default:AutoPrune", "True" },
        { "Templates:default:Recursive", "False" },
        { "Templates:default:SkipChildren", "False" },
        { "Templates:default:SnapshotTiming", null },
        { "Templates:default:SnapshotTiming:FrequentPeriod", "15" },
        { "Templates:default:SnapshotTiming:UseLocalTime", "True" },
        { "Templates:default:SnapshotTiming:HourlyMinute", "59" },
        { "Templates:default:SnapshotTiming:DailyTime", "23:59" },
        { "Templates:default:SnapshotTiming:WeeklyDay", "1" },
        { "Templates:default:SnapshotTiming:WeeklyTime", "23:59" },
        { "Templates:default:SnapshotTiming:MonthlyDay", "31" },
        { "Templates:default:SnapshotTiming:MonthlyTime", "23:59" },
        { "Templates:default:SnapshotTiming:YearlyMonth", "12" },
        { "Templates:default:SnapshotTiming:YearlyDay", "31" },
        { "Templates:default:SnapshotTiming:YearlyTime", "23:59" },
        { "Templates:default:SnapshotRetention", null },
        { "Templates:default:SnapshotRetention:Frequent", "0" },
        { "Templates:default:SnapshotRetention:Hourly", "48" },
        { "Templates:default:SnapshotRetention:Daily", "90" },
        { "Templates:default:SnapshotRetention:Weekly", "0" },
        { "Templates:default:SnapshotRetention:Monthly", "6" },
        { "Templates:default:SnapshotRetention:Yearly", "0" }
    };
}
