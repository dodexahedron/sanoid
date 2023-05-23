// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using NLog.Config;
using PowerArgs;
using Sanoid.Common.Configuration.Templates;
using Sanoid.Common.Zfs;
using Sanoid.Interop.Libc;
using Sanoid.Interop.Libc.Enums;
using Dataset = Sanoid.Common.Configuration.Datasets.Dataset;

namespace Sanoid.Common.Configuration;

/// <summary>
///     Root class for access to configuration settings
/// </summary>
public class Configuration
{
    // It is ok to disable this warning here, because we explicitly initialize everything in LoadConfigurationFromIConfiguration()
    // The code is in that method instead of this constructor because it is bad form to do things that can cause exceptions to be
    // thrown in a static constructor, if it can be reasonably avoided. So long as we make sure we've called LoadConfigurationFromIConfiguration() before
    // we touch anything else in this class, that is fine.
#pragma warning disable CS8618
    /// <summary>
    ///     Creates a new instance of a <see cref="Configuration" /> object, using the specified IConfigurationRoot
    /// </summary>
    /// <param name="rootConfiguration">The root configuration, which conforms to Sanoid.net's configuration schema</param>
    /// <param name="zfsCommandRunner">The <see cref="IZfsCommandRunner" /> that will call ZFS native commands</param>
    public Configuration( IConfigurationRoot rootConfiguration, IZfsCommandRunner zfsCommandRunner )
    {
        RootConfiguration = rootConfiguration;
        ZfsCommandRunner = zfsCommandRunner;
        //Instance = this;
    }
#pragma warning restore CS8618

    private bool _cron;

    private readonly Logger _logger = LogManager.GetCurrentClassLogger( );
    private bool _pruneSnapshots;

    internal readonly IConfigurationRoot RootConfiguration;
    private bool _takeSnapshots;
    internal readonly IZfsCommandRunner ZfsCommandRunner;

    /// <summary>
    ///     Gets or sets sanoid's cache path.<br />
    ///     Corresponds to the /CacheDirectory property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "/var/cache/sanoid"<br />
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the path for sanoid-generated cache files</value>
    public string CacheDirectory { get; set; }

    /// <summary>
    ///     Gets or sets the absolute path to the directory containing PERL sanoid's configuration files.<br />
    ///     Corresponds to the /ConfigurationPathBase property of Sanoid.json.
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
    public string ConfigurationPathBase { get; set; }

    /// <summary>
    ///     Gets or sets whether Sanoid.net should take snapshots and prune expired snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will take new snapshots and prune expired snapshots.
    /// </value>
    public bool Cron
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
    ///     Gets a <see cref="SortedDictionary{TKey,TValue}" /> of <see cref="Datasets.Dataset" />s, indexed by
    ///     <see langword="string" />.
    /// </summary>
    /// <remarks>
    ///     First element added should be the virtual root Dataset.
    /// </remarks>
    public SortedDictionary<string, Dataset> Datasets { get; } = new( );

    private IConfigurationSection DatasetsConfigurationSection => RootConfiguration.GetRequiredSection( "Datasets" );

    /// <summary>
    ///     Gets or sets the default logging levels to be used by NLog
    /// </summary>
    /// <remarks>
    ///     Getter returns the lowest severity logging level of all configured rules.<br />
    ///     Setter overrides level for all configured rules.
    /// </remarks>
    /// <value>A <see cref="LogLevel" /> indicating the lowest logging level set of any rule.</value>
    public LogLevel DefaultLoggingLevel
    {
        get
        {
            _logger.Debug( "Getting lowest log level of all rules." );
            LogLevel? lowestLogLevel = LogManager.Configuration!.LoggingRules.Min( rule => rule.Levels.Min( ) );
            if ( lowestLogLevel is null )
            {
                _logger.Debug( "No logging levels set. Setting to {0}", LogLevel.Info.Name );
                lowestLogLevel = LogLevel.Info;
            }

            _logger.Debug( "Lowest logging level is {0}", lowestLogLevel );
            return lowestLogLevel;
        }
        set
        {
            _logger.Warn( "Log levels should be changed in Sanoid.nlog.json for normal usage." );
            _logger.Debug( "Setting minimum log severity to {0} for ALL rules.", value.Name );
            for ( int ruleIndex = 0; ruleIndex < LogManager.Configuration!.LoggingRules.Count; ruleIndex++ )
            {
                LoggingRule rule = LogManager.Configuration.LoggingRules[ ruleIndex ];
                if ( value == LogLevel.Off )
                {
                    _logger.Trace( "Disabling logging for rule {0}", ruleIndex );
                    rule.SetLoggingLevels( LogLevel.Off, LogLevel.Off );
                    _logger.Trace( "Disabled logging for rule {0}", ruleIndex );
                }
                else
                {
                    _logger.Trace( "Setting log level to {0} for rule {1}", value.Name, ruleIndex );
                    rule.SetLoggingLevels( value, LogLevel.Fatal );
                    _logger.Trace( "Log level set to {0} for rule {1}", value.Name, ruleIndex );
                }
            }

            _logger.Debug( "Reconfiguring loggers" );
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
    public bool DryRun { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of threads Sanoid.net may attempt to use when taking new snapshots
    /// </summary>
    public int MaxSnapshotThreads { get; set; } = 1;

    internal ConcurrentDictionary<string, Dataset> Pools { get; } = new( );

    /// <summary>
    ///     Gets or sets whether Sanoid.net should prune expired snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will prune expired snapshots.
    /// </value>
    public bool PruneSnapshots
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
    ///     Gets or sets sanoid's run path.<br />
    ///     Corresponds to the /RunDirectory property of Sanoid.json.
    /// </summary>
    /// <remarks>
    ///     Default value is "/var/run/sanoid"<br />
    /// </remarks>
    /// <value>A <see langword="string" /> indicating the path for sanoid-generated runtime files</value>
    public string RunDirectory { get; set; }

    /// <summary>
    ///     Gets or sets the naming policy to use for snapshots.
    /// </summary>
    public SnapshotNaming SnapshotNaming { get; set; }

    /// <summary>
    ///     Gets or sets whether Sanoid.net should take new snapshots.
    /// </summary>
    /// <value>
    ///     A <see langword="bool" /> indicating whether Sanoid.net will take new snapshots.
    /// </value>
    public bool TakeSnapshots
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
    ///     Gets a <see cref="Dictionary{TKey,TValue}" /> of <see cref="Template" />s, indexed
    ///     by <see langword="string" />.
    /// </summary>
    /// <remarks>
    ///     First initialized to an empty dictionary on instantiation of the static <see cref="Configuration" /> class,
    ///     and then any <see cref="Template" />s found in Sanoid.json are added to the
    ///     collection.
    /// </remarks>
    public Dictionary<string, Template> Templates { get; } = new( );

    /// <summary>
    ///     Builds the full dataset path tree and creates datasets as disabled entries.
    /// </summary>
    private void BuildDatasetHierarchy( Template defaultTemplateForRoot )
    {
        _logger.Debug( "Building dataset hiearchy from combination of configured datasets and all datasets on system." );

        // First, add the virtual root dataset, with the provided template
        Datasets.Add( "/", Dataset.GetRoot( defaultTemplateForRoot ) );

        ImmutableSortedSet<string> zfsListResults = ZfsCommandRunner.ZfsListAll( );
        // List is returned in the form of a path tree already, so we can just scan the list linearly
        // Pool nodes will be added as children of the dummy root node, and so on down the chain until all datasets exist in the
        // Datasets dictionary
        foreach ( string dsName in zfsListResults )
        {
            _logger.Debug( "Processing dataset {0} from zfs list.", dsName );
        #if WINDOWS
            // Gotta love how Windows changes the forward slashes to backslashes silently, but only on paths more than 1 deep...
            string parentDsName = $"/{Path.GetDirectoryName( dsName )}".Replace( "\\", "/" );
        #else
            string parentDsName = $"/{Path.GetDirectoryName( dsName )}";
        #endif
            Dataset newDs = new( dsName, Datasets[ parentDsName ] );
            Datasets.TryAdd( newDs.VirtualPath, newDs );
            if ( newDs.IsPool )
            {
                Pools.TryAdd( newDs.VirtualPath, newDs );
            }

            _logger.Debug( "Dataset {0} added to dictionary.", dsName );
        }
    }

    private void LoadDatasetConfigurations( )
    {
        //TODO: This can probably be inlined when loading datasets
        _logger.Debug( "Setting dataset options from configuration" );
        // Scan the datasets collection
        // If an entry exists in configuration, set its settings, following inheritance rules.
        foreach ( ( _, Dataset? ds ) in Datasets )
        {
            _logger.Debug( "Processing dataset {0}", ds.VirtualPath );
            if ( ds.VirtualPath == "/" )
            {
                //Skip the root dataset, as it is already configured for defaults.
                continue;
            }

            IConfigurationSection section = DatasetsConfigurationSection.GetSection( ds.Path );
            if ( section.Exists( ) )
            {
                _logger.Debug( "Dataset {0} is explicitly configured.", ds.Path );
                // Dataset exists in configuration. Set configured settings and inherit everything else
                ds.IsInConfiguration = true;
                ds.Enabled = section.GetBoolean( "Enabled", true );
                string? templateName = section[ "Template" ];
                ds.Template = templateName is null ? ds.Parent!.Template : Templates[ templateName ];

                IConfigurationSection overrides = section.GetSection( "TemplateOverrides" );
                if ( overrides.Exists( ) )
                {
                    _logger.Debug( "Template overrides exist for Dataset {0}. Creating override Template with settings inherited from Template {1}.", section.Key, templateName );
                    ds.Template = ds.Template!.CloneForDatasetWithOverrides( overrides, ds, Templates );
                }
            }
            else
            {
                // Dataset is not explicitly configured. Inherit relevant properties from parent only.
                ds.Enabled = ds.Parent!.Enabled;
                ds.Template = ds.Parent.Template;
                _logger.Debug( "Dataset {0} is not explicitly configured, and is {1}enabled due to inheritance.", ds.Path, ds.Enabled ? "" : "not " );
            }

            _logger.Debug( "Finished configuring dataset {0}", ds.Path );
            _logger.Trace( "Final configuration of dataset {0}: {1}", ds.Path, JsonSerializer.Serialize( ds, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull } ) );
        }

        _logger.Debug( "Dataset options configured." );
    }

    private void BuildTemplateHierarchy( IConfigurationSection defaultTemplateConfigurationSection )
    {
        // We have enforced a tree structure for templates in the configuration, so we can recursively descend down the configuration
        // tree to get template configuration. We will add them to a flat dictionary, as well, for easy access by key.

        _logger.Debug( "Creating Template objects from configuration" );

        // First, add the default template
        Template defaultTemplate = Template.GetDefault( defaultTemplateConfigurationSection );
        Templates.TryAdd( "default", defaultTemplate );

        // Now, for all children, if any, call recursive function to descend down the tree, respecting inheritance as we go
        // This is the "Templates" section of the "default" template. It can exist and be empty, exist and have entries, or not exist.
        // All cases are ok, as the Exists() function handles that for us.
        IConfigurationSection defaultTemplateChildTemplatesConfigurationSection = defaultTemplateConfigurationSection.GetSection( "Templates" );

        if ( defaultTemplateChildTemplatesConfigurationSection.Exists( ) )
        {
            foreach ( IConfigurationSection childConfigurationSection in defaultTemplateChildTemplatesConfigurationSection.GetChildren( ) )
            {
                defaultTemplate.CreateChild( childConfigurationSection, Templates, childConfigurationSection.Key );
            }
        }

        _logger.Debug( "Templates loaded." );
    }

    /// <summary>
    ///     Loads Sanoid.net's configuration from the various sources combined in <see cref="RootConfiguration" />
    /// </summary>
    public void LoadConfigurationFromIConfiguration( )
    {
        // Global configuration initialization
        GetBaseConfiguration( );

        // Template configuration initialization
        _logger.Debug( "Initializing template configuration from Sanoid.json#/Templates" );
        // First, find the default template
        RootConfiguration.CheckTemplateSectionExists( "default", out IConfigurationSection defaultTemplateSection );
        defaultTemplateSection.CheckTemplateSnapshotRetentionSectionExists( out IConfigurationSection _ );
        defaultTemplateSection.CheckDefaultTemplateSnapshotTimingSectionExists( out IConfigurationSection _ );

        BuildTemplateHierarchy( defaultTemplateSection );
        _logger.Debug( "Template configuration complete." );

        // Diverging from PERL sanoid a bit, here.
        // We can much more efficiently call zfs list once for everything and just process the strings internally, rather
        // than invoking multiple zfs list processes.
        BuildDatasetHierarchy( Templates[ "default" ] );
        LoadDatasetConfigurations( );
    }

    private void GetBaseConfiguration( )
    {
        _logger.Debug( "Initializing root-level configuration from Sanoid.Json#/" );
        CacheDirectory = RootConfiguration[ "SanoidConfigurationCacheDirectory" ] ?? "/var/cache/sanoid";
        ConfigurationPathBase = RootConfiguration[ "SanoidConfigurationPathBase" ] ?? "/etc/sanoid";
        RunDirectory = RootConfiguration[ "SanoidConfigurationRunDirectory" ] ?? "/var/run/sanoid";
        TakeSnapshots = RootConfiguration.GetBoolean( "TakeSnapshots" );
        PruneSnapshots = RootConfiguration.GetBoolean( "PruneSnapshots" );
        SnapshotNaming = new( RootConfiguration.GetRequiredSection( "Formatting" ).GetRequiredSection( "SnapshotNaming" ) );
        _logger.Debug( "Root level configuration initialized." );
    }

    /// <summary>
    ///     Overrides configuration values specified in configuration files or environment variables with arguments supplied on
    ///     the CLI
    /// </summary>
    /// <param name="argParseReults"></param>
    public void SetValuesFromArgs( ArgAction<CommandLineArguments> argParseReults )
    {
        //TODO: Might move to using the .net configuration providers to parse the arguments, instead of PowerArgs.
        _logger.Debug( "Overriding settings from command line." );
        _logger.Trace( "Arguments object: {0}", JsonSerializer.Serialize( argParseReults.Args, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull } ) );
        // Let's go through all args in an order that makes sense
        CommandLineArguments args = argParseReults.Args;
        if ( !string.IsNullOrEmpty( args.CacheDir ) )
        {
            _logger.Debug( "CacheDir argument specified. Value: {0}.", args.CacheDir );
            string canonicalCacheDirPath = NativeMethods.CanonicalizeFileName( args.CacheDir );
            _logger.Debug( "CacheDir canonical path: {0}.", canonicalCacheDirPath );
            if ( !Directory.Exists( canonicalCacheDirPath ) )
            {
                string badDirectoryMessage = $"CacheDir argument value {canonicalCacheDirPath} is a non-existent directory. Program will terminate.";
                _logger.Error( badDirectoryMessage );
                throw new DirectoryNotFoundException( badDirectoryMessage );
            }

            if ( NativeMethods.EuidAccess( canonicalCacheDirPath, UnixFileTestMode.Read ) != 0 )
            {
                string cantReadDirMessage = $"CacheDir {canonicalCacheDirPath} is not readable by the current user {Environment.UserName}. Program will terminate.";
                _logger.Error( cantReadDirMessage );
                throw new UnauthorizedAccessException( cantReadDirMessage );
            }

            if ( NativeMethods.EuidAccess( canonicalCacheDirPath, UnixFileTestMode.Write ) != 0 )
            {
                string cantWriteDirMessage = $"CacheDir {canonicalCacheDirPath} is not writeable by the current user {Environment.UserName}. Program will terminate.";
                _logger.Error( cantWriteDirMessage );
                throw new UnauthorizedAccessException( cantWriteDirMessage );
            }

            CacheDirectory = args.CacheDir;
            _logger.Debug( "CacheDirectory is now {0}", canonicalCacheDirPath );
        }

        if ( !string.IsNullOrEmpty( args.RunDir ) )
        {
            _logger.Debug( "RunDir argument specified. Value: {0}.", args.RunDir );
            string canonicalRunDirPath = NativeMethods.CanonicalizeFileName( args.RunDir );
            _logger.Debug( "RunDir canonical path: {0}.", canonicalRunDirPath );
            if ( !Directory.Exists( canonicalRunDirPath ) )
            {
                string badDirectoryMessage = $"RunDir argument value {canonicalRunDirPath} is a non-existent directory. Program will terminate.";
                _logger.Error( badDirectoryMessage );
                throw new DirectoryNotFoundException( badDirectoryMessage );
            }

            if ( NativeMethods.EuidAccess( canonicalRunDirPath, UnixFileTestMode.Read ) != 0 )
            {
                string cantReadDirMessage = $"RunDir {canonicalRunDirPath} is not readable by the current user {Environment.UserName}. Program will terminate.";
                _logger.Error( cantReadDirMessage );
                throw new UnauthorizedAccessException( cantReadDirMessage );
            }

            if ( NativeMethods.EuidAccess( canonicalRunDirPath, UnixFileTestMode.Write ) != 0 )
            {
                string cantWriteDirMessage = $"RunDir {canonicalRunDirPath} is not writeable by the current user {Environment.UserName}. Program will terminate.";
                _logger.Error( cantWriteDirMessage );
                throw new UnauthorizedAccessException( cantWriteDirMessage );
            }

            RunDirectory = args.RunDir;
            _logger.Debug( "RunDirectory is now {0}", canonicalRunDirPath );
        }

        if ( args.TakeSnapshots is not null )
        {
            _logger.Debug( "TakeSnapshots argument specified. Value: {0}", args.TakeSnapshots );

            TakeSnapshots = args.TakeSnapshots!.Value;
            _logger.Debug( "TakeSnapshots is now {0}", TakeSnapshots );
        }
    }

    public void TrimUnwantedDatasetsFromRunningConfiguration( )
    {
        _logger.Debug( "Pruning all unwanted Datasets from running configuration." );
        string[] allDatasets = Datasets.Where( ( kvp ) => kvp.Value.IsPool ).Select( kvp => kvp.Key ).ToArray( );
        foreach ( string dsName in allDatasets )
        {
            if ( dsName == "/" )
                continue;
            Datasets.TryGetValue( dsName, out Dataset ds );
            ds!.TrimUnwantedChildren( Datasets );
            _logger.Debug( "Checking if {0} is enabled.", dsName );
            if ( !Datasets[ dsName ].Enabled )
            {
                _logger.Debug( "Dataset {0} is not enabled. Removing.", dsName );
                Datasets.Remove( dsName );
            }

            _logger.Debug( "Dataset {0} is enabled.", dsName );
        }

        _logger.Debug( "Finished pruning all unwanted Datasets from running configuration." );
    }
}
