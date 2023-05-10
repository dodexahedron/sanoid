// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using NLog.Config;
using Sanoid.Common.Configuration.Datasets;
using Sanoid.Common.Configuration.Templates;

namespace Sanoid.Common.Configuration;

/// <summary>
///     Corresponds to the root section of Sanoid.json
/// </summary>
public static class Configuration
{
    static Configuration( )
    {
        Log = LogManager.GetCurrentClassLogger( );

        // Global configuration initialization
        Log.Debug( "Initializing root-level configuration from Sanoid.Json#/" );
        SanoidConfigurationCacheDirectory = JsonConfigurationSections.RootConfiguration[ "SanoidConfigurationCacheDirectory" ] ?? "/var/cache/sanoid";
        SanoidConfigurationDefaultsFile = JsonConfigurationSections.RootConfiguration[ "SanoidConfigurationDefaultsFile" ] ?? "sanoid.defaults.conf";
        SanoidConfigurationLocalFile = JsonConfigurationSections.RootConfiguration[ "SanoidConfigurationLocalFile" ] ?? "sanoid.conf";
        SanoidConfigurationPathBase = JsonConfigurationSections.RootConfiguration[ "SanoidConfigurationPathBase" ] ?? "/etc/sanoid";
        SanoidConfigurationRunDirectory = JsonConfigurationSections.RootConfiguration[ "SanoidConfigurationRunDirectory" ] ?? "/var/run/sanoid";
        UseSanoidConfiguration = JsonConfigurationSections.RootConfiguration.GetBoolean( "UseSanoidConfiguration" );
        TakeSnapshots = JsonConfigurationSections.RootConfiguration.GetBoolean( "TakeSnapshots" );
        PruneSnapshots = JsonConfigurationSections.RootConfiguration.GetBoolean( "PruneSnapshots" );
        Log.Debug( "Root level configuration initialized." );

        // Template configuration initialization
        Log.Debug( "Initializing template configuration from Sanoid.json#/Templates" );
        // First, find the default tempate
        IConfigurationSection defaultTemplateSection;
        IConfigurationSection defaultTemplateSnapshotRetentionSection;
        IConfigurationSection defaultTemplateSnapshotTimingSection;
        try
        {
            Log.Trace( "Checking for existence of 'default' Template" );
            defaultTemplateSection = JsonConfigurationSections.TemplatesConfiguration.GetRequiredSection( "default" );
            Log.Trace( "'default' Template found" );
        }
        catch ( InvalidOperationException ex )
        {
            Log.Fatal( "Template 'default' not found in Sanoid.json#/Templates. Program will terminate.", ex );
            throw;
        }

        try
        {
            Log.Trace( "Checking for existence of 'SnapshotRetention' section in 'default' Template" );
            defaultTemplateSnapshotRetentionSection = defaultTemplateSection.GetRequiredSection( "SnapshotRetention" );
            Log.Trace( "'SnapshotRetention' section found" );
        }
        catch ( InvalidOperationException ex )
        {
            Log.Fatal( "Template 'default' does not contain the required SnapshotRetention section. Program will terminate.", ex );
            throw;
        }

        try
        {
            Log.Trace( "Checking for existence of 'SnapshotTiming' section in 'default' Template" );
            defaultTemplateSnapshotTimingSection = defaultTemplateSection.GetRequiredSection( "SnapshotTiming" );
            Log.Trace( "'SnapshotTiming' section found" );
        }
        catch ( InvalidOperationException ex )
        {
            Log.Fatal( "Template 'default' does not contain the required SnapshotTiming section. Program will terminate.", ex );
            throw;
        }

        LoadTemplates( );
        BuildTemplateHierarchy( );
        InheritImmutableTemplateSettings( defaultTemplateSnapshotRetentionSection, defaultTemplateSnapshotTimingSection );
        Log.Debug( "Template configuration complete." );

        Log.Debug( "Initializing Dataset configuration from Sanoid.json" );
        LoadConfiguredDatasets( );
        GetZfsDatasets( );
    }

    private static void GetZfsDatasets( )
    {
        Zfs.CommandRunner.ZfsListAll( );
    }

    private static void LoadConfiguredDatasets( )
    {
        Log.Debug( "Creating Dataset objects from configuration" );
        IEnumerable<IConfigurationSection> datasetSections = JsonConfigurationSections.DatasetsConfiguration.GetChildren( );
        foreach ( IConfigurationSection section in datasetSections )
        {
            Dataset newDataset = new ( section.Key )
            {
                Path = section.Key,
                Enabled = section.GetBoolean( "Enabled", true ),
                Template = Templates[section["Template"] ?? "default"]
            };
            IConfigurationSection overrides = section.GetSection( "TemplateOverrides" );
            if ( overrides.Exists( ) )
            {
                Log.Trace( "Template overrides exist for Dataset {0}. Creating override Template with settings inherited from Template {1}.", section.Key, newDataset.Template.Name );
                newDataset.Template = newDataset.Template.CloneForDatasetWithOverrides( newDataset, overrides );
            }
            Datasets.Add( section.Key, newDataset );
        }

        Log.Debug( "Configured datasets loaded." );
    }

    /// <summary>
    ///     Gets or sets whether Sanoid.net should take snapshots and prune expired snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will take new snapshots and prune expired snapshots.
    /// </value>
    [JsonPropertyName( "Cron" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    public static bool Cron
    {
        get => _cron;
        set
        {
            if ( value )
            {
                _takeSnapshots = true;
                _pruneSnapshots = true;
            }

            _cron = value;
        }
    }

    /// <summary>
    ///     Gets a <see cref="Dictionary{TKey,TValue}" /> of <see cref="Dataset" />s, indexed by <see langword="string" />.
    /// </summary>
    /// <remarks>
    ///     First initialized with a dummy root ("/") dataset on instantiation of the static <see cref="Configuration" /> class,
    ///     and then any <see cref="Dataset" />s found in Sanoid.json are added to the collection.
    /// </remarks>
    public static Dictionary<string, Dataset> Datasets { get; } = new( ) { { "/", Dataset.Root } };

    /// <summary>
    ///     Gets or sets the default logging levels to be used by NLog
    /// </summary>
    /// <remarks>
    ///     Getter returns the lowest severity logging level of all configured rules.<br />
    ///     Setter overrides level for all configured rules.
    /// </remarks>
    /// <value>A <see cref="LogLevel" /> indicating the lowest logging level set of any rule.</value>
    public static LogLevel DefaultLoggingLevel
    {
        get
        {
            Log.Debug( "Getting lowest log level of all rules." );
            LogLevel? lowestLogLevel = LogManager.Configuration!.LoggingRules.Min( rule => rule.Levels.Min( ) );
            if ( lowestLogLevel is null )
            {
                Log.Debug( "No logging levels set. Setting to {0}", LogLevel.Info.Name );
                lowestLogLevel = LogLevel.Info;
            }

            Log.Debug( "Lowest logging level is {0}", lowestLogLevel );
            return lowestLogLevel;
        }
        set
        {
            Log.Warn( "Log levels should be changed in Sanoid.nlog.json for normal usage." );
            Log.Debug( "Setting minimum log severity to {0} for ALL rules.", value.Name );
            for ( int ruleIndex = 0; ruleIndex < LogManager.Configuration!.LoggingRules.Count; ruleIndex++ )
            {
                LoggingRule rule = LogManager.Configuration!.LoggingRules[ ruleIndex ];
                if ( value == LogLevel.Off )
                {
                    Log.Trace( "Disabling logging for rule {0}", ruleIndex );
                    rule.SetLoggingLevels( LogLevel.Off, LogLevel.Off );
                    Log.Trace( "Disabled logging for rule {0}", ruleIndex );
                }
                else
                {
                    Log.Trace( "Setting log level to {0} for rule {1}", value.Name, ruleIndex );
                    rule.SetLoggingLevels( value, LogLevel.Fatal );
                    Log.Trace( "Log level set to {0} for rule {1}", value.Name, ruleIndex );
                }
            }

            Log.Debug( "Reconfiguring loggers" );
            LogManager.ReconfigExistingLoggers( );
        }
    }

    /// <summary>
    ///     Gets or sets if the current run is a dry run, where no changes will be made to zfs
    /// </summary>
    /// <value>
    ///     A A <see langword="bool" /> indicating if no changes will be made to zfs (<see langword="true" />) or if normal
    ///     processing will occur (<see langword="false" /> - default)
    /// </value>
    [JsonPropertyName( "DryRun" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static bool DryRun { get; set; }

    /// <summary>
    ///     Gets or sets whether Sanoid.net should prune expired snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will prune expired snapshots.
    /// </value>
    [JsonPropertyName( "PruneSnapshots" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    public static bool PruneSnapshots
    {
        get => _pruneSnapshots;
        set
        {
            if ( !value || !_takeSnapshots )
            {
                Cron = false;
            }

            _pruneSnapshots = value;
        }
    }

    /// <summary>
    ///     Gets or sets sanoid's cache path.<br />
    ///     Corresponds to the /SanoidConfigurationCacheDirectory property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "/var/cache/sanoid"<br />
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the path for sanoid-generated cache files</value>
    [JsonPropertyName( "SanoidConfigurationCacheDirectory" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static string SanoidConfigurationCacheDirectory { get; set; }

    /// <summary>
    ///     Gets or sets the name of the ini-formatted file inside the <see cref="SanoidConfigurationPathBase" /> folder
    ///     containing PERL sanoid's default configuration.<br />
    ///     Corresponds to the /SanoidConfigurationDefaultsFile property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "sanoid.defaults.conf"<br />
    ///     Assumed to be a file name only.
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the file name of the sanoid.defaults.conf file</value>
    [JsonPropertyName( "SanoidConfigurationDefaultsFile" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static string SanoidConfigurationDefaultsFile { get; set; }

    /// <summary>
    ///     Gets or sets the name of the ini-formatted file inside the <see cref="SanoidConfigurationPathBase" /> folder
    ///     containing PERL sanoid's local configuration.<br />
    ///     Corresponds to the /SanoidConfigurationLocalFile property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "sanoid.conf"<br />
    ///     Assumed to be a file name only.
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the file name of the sanoid.conf file</value>
    [JsonPropertyName( "SanoidConfigurationLocalFile" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static string SanoidConfigurationLocalFile { get; set; }

    /// <summary>
    ///     Gets or sets the absolute path to the directory containing PERL sanoid's configuration files.<br />
    ///     Corresponds to the /SanoidConfigurationPathBase property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "/etc/sanoid"<br />
    ///     Should not contain a trailing slash.<br />
    ///     Not guaranteed to work with a relative path. Use an absolute path.
    /// </remarks>
    /// <value>
    ///     A <see langword="string" /> indicating the absolute path to the directory containing PERL sanoid's configuration
    ///     files
    /// </value>
    [JsonPropertyName( "SanoidConfigurationPathBase" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static string SanoidConfigurationPathBase { get; set; }

    /// <summary>
    ///     Gets or sets sanoid's run path.<br />
    ///     Corresponds to the /SanoidConfigurationRunDirectory property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "/var/run/sanoid"<br />
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the path for sanoid-generated runtime files</value>
    [JsonPropertyName( "SanoidConfigurationRunDirectory" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static string SanoidConfigurationRunDirectory { get; set; }

    /// <summary>
    ///     Gets or sets whether Sanoid.net should take new snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will take new snapshots.
    /// </value>
    [JsonPropertyName( "TakeSnapshots" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    public static bool TakeSnapshots
    {
        get => _takeSnapshots;
        set
        {
            if ( !value || !_pruneSnapshots )
            {
                Cron = false;
            }

            _takeSnapshots = value;
        }
    }

    /// <summary>
    ///     Gets a <see cref="Dictionary{TKey,TValue}" /> of <see cref="Template" />s, indexed by <see langword="string" />.
    /// </summary>
    /// <remarks>
    ///     First initialized to an empty dictionary on instantiation of the static <see cref="Configuration" /> class,
    ///     and then any <see cref="Template" />s found in Sanoid.json are added to the collection.
    /// </remarks>
    public static Dictionary<string, Template> Templates { get; } = new( );

    /// <summary>
    ///     Gets or sets whether Sanoid.net should use ini-formatted configuration files using PERL sanoid's schema.<br />
    ///     Corresponds to the /UseSanoidConfiguration property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is true<br />
    ///     If <c>true</c>, uses <see cref="SanoidConfigurationDefaultsFile" /> and <see cref="SanoidConfigurationLocalFile" />
    ///     in the <see cref="SanoidConfigurationPathBase" /> directory.<br />
    ///     If <c>false</c>, uses configuration in Sanoid.json only.
    /// </remarks>
    /// <value>
    ///     A <see langword="bool" /> indicating whether PERL sanoid's configuration will be respected (
    ///     <see langword="true" />) or not (<see langword="false" />).
    /// </value>
    [JsonPropertyName( "UseSanoidConfiguration" )]
    [JsonIgnore( Condition = JsonIgnoreCondition.Never )]
    [JsonRequired]
    public static bool UseSanoidConfiguration { get; [NotNull] set; }

    private static bool _cron;
    private static bool _pruneSnapshots;
    private static bool _takeSnapshots;

    private static readonly Logger Log;

    private static void LoadTemplates( )
    {
        Log.Debug( "Creating Template objects from configuration" );
        IEnumerable<IConfigurationSection> templateSections = JsonConfigurationSections.TemplatesConfiguration.GetChildren( );
        foreach ( IConfigurationSection templateSection in templateSections )
        {
            bool isDefaultSection = templateSection.Key == "default";
            Template newTemplate = new( templateSection.Key, templateSection[ "UseTemplate" ] ?? "default" )
            {
                AutoPrune = templateSection.GetBoolean( "AutoPrune", isDefaultSection ? true : null ),
                AutoSnapshot = templateSection.GetBoolean( "AutoSnapshot", isDefaultSection ? true : null ),
                Recursive = templateSection.GetBoolean( "Recursive", isDefaultSection ? false : null ),
                SkipChildren = templateSection.GetBoolean( "SkipChildrn", isDefaultSection ? false : null )
            };
            Templates.Add( templateSection.Key, newTemplate );
        }

        Log.Debug( "Templates loaded." );
    }

    /// <summary>
    ///     Builds the template hierarchy and assigns <see cref="SnapshotRetention" /> and <see cref="SnapshotTiming" />
    ///     settings.
    /// </summary>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="ConfigurationValidationException"></exception>
    private static void BuildTemplateHierarchy( )
    {
        // To build the inheritance hierarchy, scan the dictionary and set references for UseTemplate
        // Note that the setter for Template.UseTemplate automatically adds the template to its parent's Children collection
        Log.Debug( "Building template hierarchy." );
        foreach ( ( string key, Template value ) in Templates )
        {
            Log.Trace( "Attempting to assign template {0} as parent of template {1}", value.UseTemplateName, key );
            if ( key == "default" )
            {
                Log.Trace( "Default template. Not assigning a parent." );
                continue;
            }

            if ( string.IsNullOrWhiteSpace( value.UseTemplateName ) )
            {
                Log.Fatal( "UseTemplate value specified for Template {0} must refer to an existing template name. Program will terminate.", key );
                throw new ConfigurationValidationException( $"UseTemplate value specified for Template {key} must refer to an existing template name. Program will terminate." );
            }

            if ( Templates.TryGetValue( value.UseTemplateName, out Template? parentTemplate ) )
            {
                Log.Trace( "Parent template {0} of template {1} found. Assigning parent." );
                value.UseTemplate = parentTemplate;
            }
            else
            {
                Log.Fatal( "Parent template {0} of template {1} not defined in Sanoid.json. Program will terminate.", value.UseTemplateName, key );
                throw new KeyNotFoundException( $"Parent template {value.UseTemplateName} of template {value.Name} not defined in configuration." );
            }
        }

        Log.Debug( "Template hierarchy built." );
    }

    private static void InheritImmutableTemplateSettings( IConfigurationSection retention, IConfigurationSection timing )
    {
        // First, set up the default template, as it is the root of inheritance
        Templates[ "default" ].SnapshotRetention = new SnapshotRetention
        {
            Frequent = retention.GetInt( "Frequent" ),
            Monthly = retention.GetInt( "Monthly" ),
            PruneDeferral = retention.GetInt( "PruneDeferral" ),
            Weekly = retention.GetInt( "Weekly" ),
            Yearly = retention.GetInt( "Yearly" ),
            FrequentPeriod = retention.GetInt( "FrequentPeriod" ),
            Daily = retention.GetInt( "Daily" ),
            Hourly = retention.GetInt( "Hourly" )
        };
        Templates[ "default" ].SnapshotTiming = new SnapshotTiming
        {
            DailyTime = TimeOnly.Parse( timing[ "DailyTime" ]! ),
            HourlyMinute = timing.GetInt( "HourlyMinute" ),
            MonthlyDay = timing.GetInt( "MonthlyDay" ),
            MonthlyTime = TimeOnly.Parse( timing[ "MonthlyTime" ]! ),
            UseLocalTime = timing.GetBoolean( "UseLocalTime", true ),
            WeeklyDay = (DayOfWeek)timing.GetInt( "WeeklyDay" ),
            WeeklyTime = TimeOnly.Parse( timing[ "WeeklyTime" ]! ),
            YearlyDay = timing.GetInt( "YearlyDay" ),
            YearlyMonth = timing.GetInt( "YearlyMonth" ),
            YearlyTime = TimeOnly.Parse( timing[ "YearlyTime" ]! )
        };

        // Now recursively set all children's settings
        Templates[ "default" ].InheritSnapshotRetentionAndTimingSettings( );
    }

    /// <summary>
    ///     Method used to force instatiation of this static class.
    /// </summary>
    public static void Initialize( )
    {
        Log.Trace( "Initializing configuration." );
    }
}
