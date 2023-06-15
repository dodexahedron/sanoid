// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

#nullable enable

using System.Text.Json;
using System.Text.Json.Serialization;
using Sanoid.Settings.Settings;

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v1.0.24.0
//      You can make changes to this file and they will not be overwritten when saving.
//  </auto-generated>
// -----------------------------------------------------------------------------

namespace Sanoid.ConfigConsole
{
    using System;
    using Terminal.Gui;

    public partial class SanoidConfigConsole
    {
        private GlobalConfigurationWindow? _globalConfigurationWindow;
        private ZfsConfigurationWindow? _zfsConfigurationWindow;
        private TemplateConfigurationWindow? _templateConfigurationWindow;
        public SanoidConfigConsole( )
        {
            Initialized+= SanoidConfigConsoleOnInitialized;
            Ready += SanoidConfigConsoleOnReady;
            InitializeComponent( );
            globalConfigMenuItem.Action = ShowGlobalConfigurationWindow;
            templateConfigMenuItem.Action = ShowTemplateConfigurationWindow;
            zfsConfigMenuItem.Action = ShowZfsConfigurationWindow;
            saveMenuItem.Action = ShowSaveDialog;
        }
        private void ShowSaveDialog( )
        {
            try
            {
                DisableEventHandlers( );
                if ( _globalConfigurationWindow is null )
                {
                    Logger.Warn( "Save configuration requested when no changes were made." );
                    int messageBoxResult = MessageBox.Query( "Are You Sure?", "No changes have been made to global configuration. Save anyway?", "Cancel", "Save" );
                    if ( messageBoxResult == 0 )
                    {
                        return;
                    }

                    SanoidSettings copyOfCurrentSettings = Program.Settings! with { };
                    (bool status, string reasonOrFile) copyConfigResult = ContinueWithSave( copyOfCurrentSettings );
                    if ( copyConfigResult.status )
                    {
                        Logger.Info( "Copy of existing configuration saved to {0}", copyConfigResult.Item2 );
                        return;
                    }

                    switch ( copyConfigResult.reasonOrFile )
                    {
                        case "canceled":
                            Logger.Debug( "Canceled configuration save dialog" );
                            return;
                        case "no file name":
                            Logger.Error( "No file name provided in save dialog. Configuration copy not saved." );
                            return;
                        default:
                            Logger.Error( "Failed to save copy of configuration." );
                            return;
                    }
                }

                if ( !_globalConfigurationWindow.ValidateGlobalConfigValues( ) )
                {
                    Logger.Warn("Global configuration input validation failed. Configuration not saved.");
                    MessageBox.ErrorQuery( "Invalid Global Configuration", "One or more entries in the global configuration window are invalid. Correct any invalid entries and try again.", "OK" );
                    return;
                }

                SanoidSettings settingsFromGlobalConfigWindow = new( )
                {
                    DryRun = _globalConfigurationWindow.dryRunRadioGroup.GetSelectedBooleanFromLabel( ),
                    TakeSnapshots = _globalConfigurationWindow.takeSnapshotsRadioGroup.GetSelectedBooleanFromLabel( ),
                    PruneSnapshots = _globalConfigurationWindow.pruneSnapshotsRadioGroup.GetSelectedBooleanFromLabel( ),
                    ZfsPath = _globalConfigurationWindow.pathToZfsTextField.Text.ToString( )!,
                    ZpoolPath = _globalConfigurationWindow.pathToZpoolTextField.Text.ToString( )!,
                    Templates = Program.Settings!.Templates,
                    CacheDirectory = Program.Settings.CacheDirectory
                };

                (bool status, string reasonOrFile) = ContinueWithSave( settingsFromGlobalConfigWindow );

                if ( status )
                {
                    Logger.Info( "Configuration saved to {0}", reasonOrFile );
                    return;
                }

                switch ( reasonOrFile )
                {
                    case "canceled":
                        Logger.Debug( "Canceled configuration save dialog" );
                        return;
                    case "no file name":
                        Logger.Error( "No file name provided in save dialog. Configuration not saved." );
                        return;
                    default:
                        Logger.Error( "Failed to save configuration." );
                        return;
                }
            }
            finally
            {
                EnableEventHandlers( );
            }

            static (bool,string) ContinueWithSave( SanoidSettings settings )
            {
                using ( SaveDialog globalConfigSaveDialog = new( "Save Global Configuration", "Select file to save global configuration", new( ) { ".json" } ) )
                {
                    globalConfigSaveDialog.DirectoryPath = "/etc/sanoid";
                    globalConfigSaveDialog.AllowsOtherFileTypes = true;
                    globalConfigSaveDialog.CanCreateDirectories = true;
                    globalConfigSaveDialog.Modal = true;
                    Application.Run( globalConfigSaveDialog );
                    if ( globalConfigSaveDialog.Canceled )
                    {
                        return ( false, "canceled" );
                    }

                    if ( globalConfigSaveDialog.FileName.IsEmpty )
                    {
                        return (false,"no file name");
                    }

                    string path = globalConfigSaveDialog.FilePath.ToString( ) ?? throw new InvalidOperationException( "Null string provided for save file name" );

                    if ( File.Exists( path ) )
                    {
                        int overwriteResult = MessageBox.ErrorQuery( "Overwrite Existing File?", $"The file '{path}' already exists. Continue saving and overwrite this file?", "Cancel", "Overwrite" );
                        if ( overwriteResult == 0 )
                        {
                            return ( false, "canceled" );
                        }
                    }

                    File.WriteAllText( path, JsonSerializer.Serialize( settings, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never } ) );
                    return ( true, path );
                }
            }
        }


        private void ShowGlobalConfigurationWindow( )
        {
            _globalConfigurationWindow ??= new( );
            Add( _globalConfigurationWindow );
            if ( ShowChild( _globalConfigurationWindow ) )
            {
                Logger.Debug("Showing global configuration window");
                globalConfigMenuItem.Action = HideGlobalConfigurationWindow;
            }
            else
            {
                Remove( _globalConfigurationWindow );
                Logger.Error( "Unable to show global configuration window" );
            }
        }

        private void HideGlobalConfigurationWindow( )
        {
            Remove( _globalConfigurationWindow );
            globalConfigMenuItem.Action = ShowGlobalConfigurationWindow;
        }

        private void ShowTemplateConfigurationWindow( )
        {
            _templateConfigurationWindow ??= new( );
            Add( _templateConfigurationWindow );
            if ( ShowChild( _templateConfigurationWindow ) )
            {
                Logger.Debug( "Showing template configuration window" );
                templateConfigMenuItem.Action = HideTemplateConfigurationWindow;
            }
            else
            {
                Logger.Error( "Unable to show template configuration window" );
            }
        }

        private void HideTemplateConfigurationWindow()
        {
            Remove( _templateConfigurationWindow );
            templateConfigMenuItem.Action = ShowTemplateConfigurationWindow;
        }

        private void ShowZfsConfigurationWindow( )
        {
            _zfsConfigurationWindow ??= new( );
            Add( _zfsConfigurationWindow );
            if ( ShowChild( _zfsConfigurationWindow ) )
            {
                Logger.Debug( "Showing ZFS configuration window" );
                zfsConfigMenuItem.Action = HideZfsConfigurationWindow;
            }
            else
            {
                Logger.Error( "Unable to show ZFS configuration window" );
            }
        }

        private void HideZfsConfigurationWindow( )
        {
            Remove( _zfsConfigurationWindow );
            zfsConfigMenuItem.Action = ShowZfsConfigurationWindow;
        }

        private void SanoidConfigConsoleOnInitialized( object? sender, EventArgs e )
        {
            AddKeyBinding( Key.CtrlMask | Key.q, Command.QuitToplevel );
            quitMenuItem.Action = Application.Top.RequestStop;
            IsMdiContainer = true;
            Logger.Fatal( Application.MdiTop.Text );
        }

        private bool _eventsEnabled;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger( );

        private void SanoidConfigConsoleOnReady( )
        {
            EnableEventHandlers( );
        }

        private void GetAllSubviewTabInfo<T>( T startingPoint ) where T : View
        {
            //Logger.Warn( startingPoint.ToString( ) );
            //Logger.Warn( JsonSerializer.Serialize( new { TabStop = startingPoint.TabStop, TabIndex = startingPoint.TabIndex, TabIndexes = startingPoint.TabIndexes.Select( t => new { Type = t.GetType( ).Name, Index = t.TabIndex } ).ToArray( ) } ) );
            if ( startingPoint.Subviews.Count > 0 )
            {
                foreach ( var subview in startingPoint.Subviews )
                {
                    GetAllSubviewTabInfo( subview );
                }
            }
        }

        private void DisableEventHandlers( )
        {
            if ( !_eventsEnabled )
            {
                return;
            }

            _eventsEnabled = false;
        }

        private void EnableEventHandlers( )
        {
            if ( _eventsEnabled )
            {
                return;
            }

            _eventsEnabled = true;
        }
    }
}
