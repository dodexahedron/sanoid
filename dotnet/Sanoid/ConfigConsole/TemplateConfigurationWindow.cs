// LICENSE:
// 
// This software is licensed for use under the Free Software Foundation's GPL v3.0 license, as retrieved
// from http://www.gnu.org/licenses/gpl-3.0.html on 2014-11-17.  A copy should also be available in this
// project's Git repository at https://github.com/jimsalterjrs/sanoid/blob/master/LICENSE.

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v1.0.24.0
//      You can make changes to this file and they will not be overwritten when saving.
//  </auto-generated>
// -----------------------------------------------------------------------------
#nullable enable

using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NStack;
using Sanoid.Interop.Zfs.ZfsTypes;
using Sanoid.Settings.Settings;
using Terminal.Gui.TextValidateProviders;

namespace Sanoid.ConfigConsole
{
    using Terminal.Gui;

    public partial class TemplateConfigurationWindow
    {
        public TemplateConfigurationWindow( )
        {
            InitializeComponent( );
            InitializeTemplateEditorView( );
        }

        private bool _templateConfigurationEventsEnabled;
        private bool _templateConfigurationTemplatePropertiesModified;
        private bool _templatesAddedOrRemoved;
        private readonly List<TextValidateFieldSettings> _templateConfigurationTextValidateFieldList = new( );
        private static bool IsAnyTemplateModified => ConfigConsole.TemplateListItems.Any( t => t.IsModified );
        private bool IsEveryPropertyTextValidateFieldValid => _templateConfigurationTextValidateFieldList.TrueForAll( tvf => tvf.Field.IsValid );
        private bool IsSelectedTemplateInUse => ConfigConsole.BaseDatasets.Any( kvp => kvp.Value.Template.Value == SelectedTemplateItem.TemplateName );
        private TemplateConfigurationListItem SelectedTemplateItem => ConfigConsole.TemplateListItems[ templateListView.SelectedItem ];

        [NotNull]
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger( );

        private static readonly List<int> TemplateConfigurationFrequentPeriodOptions = new( ) { 5, 10, 15, 20, 30 };
        private static ConcurrentDictionary<string, TemplateSettings> Templates = new( );

        private void InitializeTemplateEditorView( )
        {
            DisableEventHandlers( );
            //HidePropertiesFrame( );
            templateListView.SetSource( ConfigConsole.TemplateListItems );
            Templates.Clear( );
            Templates = new( Program.Settings!.Templates );
            Templates.TryRemove( "default", out _ );
            InitializeComboBoxes( );
            SetInitialButtonState( );
            InitializeTemplatePropertiesTextValidateFieldList( );
            SetValidateOnInputForAllTextValidateFields( );
            UpdateTemplateListButtonStates( );
            UpdateTemplatePropertiesButtonStates( );
            EnableEventHandlers( );
        }

        private void InitializeComboBoxes( )
        {
            comboBox.SetSource( DateTimeFormatInfo.CurrentInfo.MonthNames );
        }

        private void SetInitialButtonState( )
        {
            deleteTemplateButton.Enabled = false;
            resetCurrentButton.Enabled = false;
            applyCurrentButton.Enabled = false;
        }

        private void InitializeTemplatePropertiesTextValidateFieldList( )
        {
            _templateConfigurationTextValidateFieldList.Clear( );
            _templateConfigurationTextValidateFieldList.Add( new( namingComponentSeparatorValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingPrefixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingFrequentSuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingHourlySuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingDailySuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingWeeklySuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingMonthlySuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( namingYearlySuffixTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( hourlyMinuteTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( timingWeeklyDayTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( timingMonthlyDayTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( timingYearlyMonthTextValidateField, true ) );
            _templateConfigurationTextValidateFieldList.Add( new( timingYearlyDayTextValidateField, true ) );
        }

        private void SetValidateOnInputForAllTextValidateFields( )
        {
            _templateConfigurationTextValidateFieldList.ForEach( item => ( (TextRegexProvider)item.Field.Provider ).ValidateOnInput = item.ValidateOnInput );
            ( (TextRegexProvider)newTemplateNameTextValidateField.Provider ).ValidateOnInput = false;
        }

        private void EnableEventHandlers( )
        {
            if ( _templateConfigurationEventsEnabled )
            {
                return;
            }

            templateListView.SelectedItemChanged += TemplateListViewOnSelectedItemChanged;
            addTemplateButton.Clicked += AddTemplateButtonOnClicked;
            deleteTemplateButton.Clicked += DeleteDeleteTemplateButtonOnClicked;
            applyCurrentButton.Clicked += ApplyCurrentButtonOnClicked;
            newTemplateNameTextValidateField.KeyPress += NewTemplateNameTextValidateFieldOnKeyPress;
            saveAllButton.Clicked += TemplateSettingsSaveAllButtonOnClicked;
            resetCurrentButton.Clicked += ResetCurrentButtonOnClicked;
            hourlyMinuteTextValidateField.Leave += HourlyMinuteTextValidateFieldOnLeave;
            dailyTimeTimeField.Leave += DailyTimeTimeFieldOnLeave;
            hourlyMinuteTextValidateField.KeyPress += HourlyMinuteTextValidateFieldOnKeyPress;
            dailyTimeTimeField.KeyPress += DailyTimeTimeFieldOnKeyPress;
            comboBox.SelectedItemChanged += ComboBoxOnSelectedItemChanged;
            _templateConfigurationEventsEnabled = true;
        }

        private void ApplyCurrentButtonOnClicked( )
        {
            if ( !IsEveryPropertyTextValidateFieldValid )
            {
                SelectedTemplateItem.ViewSettings.SnapshotTiming = SelectedTemplateItem.ViewSettings.SnapshotTiming with
                {
                    DailyTime = TimeOnly.FromTimeSpan( dailyTimeTimeField.Time ),
                    WeeklyTime = TimeOnly.FromTimeSpan( weeklyTimeTimeField.Time ),
                    MonthlyTime = TimeOnly.FromTimeSpan( monthlyTimeTimeField.Time ),
                    YearlyTime = TimeOnly.FromTimeSpan( yearlyTimeTimeField.Time )
                };
            }
        }

        private void DailyTimeTimeFieldOnKeyPress( KeyEventEventArgs args )
        {
            bool isTimeValueDifferent = SelectedTemplateItem.ViewSettings.SnapshotTiming.DailyTime != dailyTimeTimeField.Time.ToTimeOnly( );
            applyCurrentButton.Enabled = isTimeValueDifferent && IsEveryPropertyTextValidateFieldValid;
        }

        private void HourlyMinuteTextValidateFieldOnKeyPress( KeyEventEventArgs args )
        {
            bool fieldIsValid = hourlyMinuteTextValidateField.IsValid;
            bool isMinuteValueDifferent = SelectedTemplateItem.ViewSettings.SnapshotTiming.HourlyMinute != hourlyMinuteTextValidateField.Text.ToInt32( int.MinValue );
            applyCurrentButton.Enabled = fieldIsValid && isMinuteValueDifferent && IsEveryPropertyTextValidateFieldValid;
        }

        private void NewTemplateNameTextValidateFieldOnKeyPress( KeyEventEventArgs args )
        {
            if ( !newTemplateNameTextValidateField.Text.IsEmpty && newTemplateNameTextValidateField.IsValid )
            {
                string newTemplateName = newTemplateNameTextValidateField.Text.ToString( )!;
                addTemplateButton.Enabled = newTemplateName != "default" && !Templates.ContainsKey( newTemplateName );
            }
            else
            {
                addTemplateButton.Enabled = false;
            }
        }

        /// <exception cref="ApplicationException">
        ///     If removal of the selected template from <see cref="Templates" /> fails.<br />
        ///     Should be treated as fatal by any consumers
        /// </exception>
        private void DeleteDeleteTemplateButtonOnClicked( )
        {
            try
            {
                DisableEventHandlers( );
                string templateName = SelectedTemplateItem.TemplateName;
                Logger.Debug( "Validating selected template {0} for removal", templateName );
                if ( templateName == "default" )
                {
                    const string errorMessage = "Cannot delete the default template.";
                    Logger.Warn( errorMessage );
                    MessageBox.ErrorQuery( "Cannot Delete Template", errorMessage, 0, "OK" );
                    return;
                }

                if ( IsSelectedTemplateInUse )
                {
                    string errorMessage = $"Selected template {templateName} is in use by one or more objects in ZFS. Cannot delete template.";
                    Logger.Warn( errorMessage );
                    MessageBox.ErrorQuery( "Cannot Delete Template", errorMessage, 0, "OK" );
                    return;
                }

                Logger.Debug( "Template {0} can be removed. Removing", templateName );

                // Grab the currently selected index, decrement the selection in the ListView,
                // and remove the saved index from the source collection
                int indexToRemove = templateListView.SelectedItem;
                templateListView.SelectedItem -= 1;
                templateListView.EnsureSelectedItemVisible( );
                ConfigConsole.TemplateListItems.RemoveAt( indexToRemove );
                if ( !Templates.TryRemove( templateName, out _ ) )
                {
                    // The application state is inconsistent if this happens, and it isn't safe to continue
                    string errorMessage = $"Failed to remove template {templateName} from UI dictionary";
                    TemplateRemovalException ex = new( errorMessage );
                    Logger.Fatal( ex, errorMessage );
                    throw ex;
                }

                _templatesAddedOrRemoved = true;
            }
            catch ( ApplicationException )
            {
                // If we threw the KeyNotFoundException above, we need to exit.
                // Also, re-throw so we can deal with it in the caller as well.
                Application.Top.RequestStop( );
                throw;
            }

            UpdateTemplateListButtonStates( );
            EnableEventHandlers( );
        }

        private void AddTemplateButtonOnClicked( )
        {
            if ( !Templates.TryGetValue( "default", out _ ) )
            {
                string errorMessage = "'default' template does not exist. Not creating new template.";
                Logger.Error( errorMessage );
                MessageBox.ErrorQuery( "Error Adding Template", errorMessage, 0, "OK" );
                return;
            }

            if ( !newTemplateNameTextValidateField.IsValid )
            {
                string errorMessage = "New template name not valid. Not creating new template.";
                Logger.Error( errorMessage );
                MessageBox.ErrorQuery( "Error Adding Template", errorMessage, 0, "OK" );
                return;
            }

            string? newTemplateName = newTemplateNameTextValidateField.Text.ToString( );
            if ( Templates.ContainsKey( newTemplateName! ) )
            {
                string errorMessage = $"A template named {newTemplateName} already exists.";
                Logger.Error( errorMessage );
                MessageBox.ErrorQuery( "Error Adding Template", errorMessage, 0, "OK" );
                return;
            }

            Templates[ newTemplateName! ] = SelectedTemplateItem.ViewSettings with { };
            ConfigConsole.TemplateListItems.Add( new( newTemplateName!, SelectedTemplateItem.ViewSettings with { }, SelectedTemplateItem.ViewSettings with { } ) );
            _templatesAddedOrRemoved = true;
            UpdateTemplateListButtonStates( );
            UpdateTemplatePropertiesButtonStates( );
        }

        private void DailyTimeTimeFieldOnLeave( FocusEventArgs args )
        {
            UpdateTemplatePropertiesButtonStates( );
        }

        private void HourlyMinuteTextValidateFieldOnLeave( FocusEventArgs args )
        {
            if ( hourlyMinuteTextValidateField.IsValid )
            {
                if ( !int.TryParse( hourlyMinuteTextValidateField.Text.ToString( ), out int hourlyMinute ) && hourlyMinute is < 0 or > 59 )
                {
                    const string errorMessage = "The value entered for Hourly Minute is invalid. Field will be reset to previous value.";
                    Logger.Warn( errorMessage );
                    MessageBox.ErrorQuery( "Invalid Hourly Minute Value", errorMessage, "OK" );
                    hourlyMinuteTextValidateField.Text = SelectedTemplateItem.ViewSettings.SnapshotTiming.HourlyMinute.ToString( "D2" );
                    return;
                }

                SelectedTemplateItem.ViewSettings.SnapshotTiming = SelectedTemplateItem.ViewSettings.SnapshotTiming with { HourlyMinute = hourlyMinute };
            }

            UpdateTemplateListButtonStates( );
        }

        private void ResetCurrentButtonOnClicked( )
        {
            DisableEventHandlers( );
            SelectedTemplateItem.ViewSettings = SelectedTemplateItem.BaseSettings with { };
            SetFieldsForSelectedItem( );
            UpdateTemplatePropertiesButtonStates( );
            UpdateTemplateListButtonStates( );
            EnableEventHandlers( );
        }

        private void UpdateTemplateListButtonStates( )
        {
            saveAllButton.Enabled = ( _templatesAddedOrRemoved || IsAnyTemplateModified ) && IsEveryPropertyTextValidateFieldValid;
            deleteTemplateButton.Enabled = templateListView.SelectedItem >= 0 && !IsSelectedTemplateInUse;
            addTemplateButton.Enabled = newTemplateNameTextValidateField.IsValid;
        }

        private void UpdateTemplatePropertiesButtonStates( )
        {
            resetCurrentButton.Enabled = SelectedTemplateItem.IsModified;
            applyCurrentButton.Enabled = SelectedTemplateItem.IsModified;
        }

        private void HidePropertiesFrame( )
        {
            propertiesFrame.Visible = false;
        }

        private void ShowPropertiesFrame( )
        {
            propertiesFrame.Visible = true;
        }

        private void TemplateListViewOnSelectedItemChanged( ListViewItemEventArgs args )
        {
            DisableEventHandlers( );

            templateListView.EnsureSelectedItemVisible( );

            SetFieldsForSelectedItem( );

            UpdateTemplateListButtonStates( );

            EnableEventHandlers( );
        }

        private void SetFieldsForSelectedItem( )
        {
            if ( !propertiesFrame.Visible )
            {
                ShowPropertiesFrame( );
            }

            TemplateConfigurationListItem item = SelectedTemplateItem;
            namingComponentSeparatorValidateField.Text = ustring.Make( item.ViewSettings.Formatting.ComponentSeparator );
            namingPrefixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.Prefix );
            namingFrequentSuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.FrequentSuffix );
            namingHourlySuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.HourlySuffix );
            namingDailySuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.DailySuffix );
            namingWeeklySuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.WeeklySuffix );
            namingMonthlySuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.MonthlySuffix );
            namingYearlySuffixTextValidateField.Text = ustring.Make( item.ViewSettings.Formatting.YearlySuffix );
            namingTimestampFormatTextField.Text = ustring.Make( item.ViewSettings.Formatting.TimestampFormatString );
            frequentPeriodRadioGroup.SelectedItem = TemplateConfigurationFrequentPeriodOptions.IndexOf( item.ViewSettings.SnapshotTiming.FrequentPeriod );
            hourlyMinuteTextValidateField.Text = item.ViewSettings.SnapshotTiming.HourlyMinute.ToString( "D2" );
            dailyTimeTimeField.Time = item.ViewSettings.SnapshotTiming.DailyTime.ToTimeSpan( );
            timingWeeklyDayTextValidateField.Text = DateTimeFormatInfo.CurrentInfo.GetDayName( item.ViewSettings.SnapshotTiming.WeeklyDay );
            weeklyTimeTimeField.Time = item.ViewSettings.SnapshotTiming.WeeklyTime.ToTimeSpan( );
            timingMonthlyDayTextValidateField.Text = item.ViewSettings.SnapshotTiming.MonthlyDay.ToString( );
            monthlyTimeTimeField.Time = item.ViewSettings.SnapshotTiming.MonthlyTime.ToTimeSpan( );
            timingYearlyMonthTextValidateField.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName( item.ViewSettings.SnapshotTiming.YearlyMonth );
            timingYearlyDayTextValidateField.Text = item.ViewSettings.SnapshotTiming.YearlyDay.ToString( );
            yearlyTimeTimeField.Time = item.ViewSettings.SnapshotTiming.YearlyTime.ToTimeSpan( );
            comboBox.SelectedItem = item.ViewSettings.SnapshotTiming.YearlyMonth;
        }

        private void TemplateSettingsSaveAllButtonOnClicked( )
        {
            TemplateConfigurationValidator validator = new( );

            if ( validator.ValidateFieldValues( this ) is { IsValid: false, ValidationExceptions: { Count: > 0 } } result )
            {
                TemplateValidationException ex = new( "One or more template fields failed to validate", result.ValidationExceptions );
                Logger.Warn( ex );
                return;
            }

            SelectedTemplateItem.ViewSettings = SelectedTemplateItem.ViewSettings with
            {
                Formatting = new( )
                {
                    ComponentSeparator = validator.NamingComponentSeparator!,
                    Prefix = validator.NamingPrefix!,
                    TimestampFormatString = validator.NamingTimestampFormatString!,
                    FrequentSuffix = validator.NamingFrequentSuffix!,
                    HourlySuffix = validator.NamingHourlySuffix!,
                    DailySuffix = validator.NamingDailySuffix!,
                    WeeklySuffix = validator.NamingWeeklySuffix!,
                    MonthlySuffix = validator.NamingMonthlySuffix!,
                    YearlySuffix = validator.NamingYearlySuffix!
                },
                SnapshotTiming = SelectedTemplateItem.ViewSettings.SnapshotTiming with
                {
                    FrequentPeriod = validator.TimingFrequentPeriod!.Value,
                    HourlyMinute = validator.TimingHourlyMinute!.Value,
                    DailyTime = validator.TimingDailyTime,
                    WeeklyDay = validator.TimingWeeklyDay!.Value,
                    WeeklyTime = validator.TimingWeeklyTime,
                    MonthlyDay = validator.TimingMonthlyDay!.Value,
                    MonthlyTime = validator.TimingMonthlyTime,
                    YearlyMonth = validator.TimingYearlyMonth!.Value,
                    YearlyDay = validator.TimingYearlyDay!.Value,
                    YearlyTime = validator.TimingYearlyTime
                }
            };

            if ( SelectedTemplateItem.IsModified )
            {
                Templates[ SelectedTemplateItem.TemplateName ] = SelectedTemplateItem.ViewSettings;
                TemplateConfigurationShowSaveDialog( );
            }

            UpdateTemplateListButtonStates( );
        }

        private void TemplateConfigurationShowSaveDialog( )
        {
            using ( SaveDialog saveDialog = new( "Save Global Configuration", "Select file to save global configuration", new( ) { ".json" } ) )
            {
                saveDialog.AllowsOtherFileTypes = true;
                saveDialog.CanCreateDirectories = true;
                saveDialog.Modal = true;
                Application.Run( saveDialog );
                if ( saveDialog.Canceled )
                {
                    return;
                }

                if ( saveDialog.FileName.IsEmpty )
                {
                    return;
                }

                SanoidSettings settings = Program.Settings! with
                {
                    Templates = Templates.ToDictionary( kvp => kvp.Key, kvp => kvp.Value )
                };
                SelectedTemplateItem.BaseSettings = SelectedTemplateItem.ViewSettings with { };
                Program.Settings = settings with { };

                File.WriteAllText( saveDialog.FileName.ToString( ) ?? throw new InvalidOperationException( "Null string provided for save file name" ), JsonSerializer.Serialize( Program.Settings, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never } ) );
            }

            UpdateTemplateListButtonStates( );
        }

        private void DisableEventHandlers( )
        {
            if ( !_templateConfigurationEventsEnabled )
            {
                return;
            }

            templateListView.SelectedItemChanged -= TemplateListViewOnSelectedItemChanged;
            addTemplateButton.Clicked -= AddTemplateButtonOnClicked;
            deleteTemplateButton.Clicked -= DeleteDeleteTemplateButtonOnClicked;
            newTemplateNameTextValidateField.KeyPress -= NewTemplateNameTextValidateFieldOnKeyPress;
            saveAllButton.Clicked -= TemplateSettingsSaveAllButtonOnClicked;
            resetCurrentButton.Clicked -= ResetCurrentButtonOnClicked;
            hourlyMinuteTextValidateField.Leave -= HourlyMinuteTextValidateFieldOnLeave;
            dailyTimeTimeField.Leave -= DailyTimeTimeFieldOnLeave;
            hourlyMinuteTextValidateField.KeyPress -= HourlyMinuteTextValidateFieldOnKeyPress;
            dailyTimeTimeField.KeyPress -= DailyTimeTimeFieldOnKeyPress;
            comboBox.SelectedItemChanged -= ComboBoxOnSelectedItemChanged;
            _templateConfigurationEventsEnabled = false;
        }

        private void ComboBoxOnSelectedItemChanged( ListViewItemEventArgs args )
        {
            SelectedTemplateItem.ViewSettings.SnapshotTiming = SelectedTemplateItem.ViewSettings.SnapshotTiming with
            {
                YearlyMonth = args.Item + 1
            };
            UpdateTemplatePropertiesButtonStates( );
        }

        private record TextValidateFieldSettings( TextValidateField Field, bool ValidateOnInput );

        private class TemplateConfigurationValidator
        {
            public string? NamingComponentSeparator { get; private set; }
            public string? NamingDailySuffix { get; private set; }
            public string? NamingFrequentSuffix { get; private set; }
            public string? NamingHourlySuffix { get; private set; }
            public string? NamingMonthlySuffix { get; private set; }
            public string? NamingPrefix { get; private set; }
            public string? NamingTimestampFormatString { get; private set; }
            public string? NamingWeeklySuffix { get; private set; }
            public string? NamingYearlySuffix { get; private set; }
            private TemplateConfigurationWindow? TemplateWindow { get; set; }
            public TimeOnly TimingDailyTime { get; private set; }
            public int? TimingFrequentPeriod { get; private set; }
            public int? TimingHourlyMinute { get; private set; }
            public int? TimingMonthlyDay { get; private set; }
            public TimeOnly TimingMonthlyTime { get; private set; }
            public DayOfWeek? TimingWeeklyDay { get; private set; }
            public TimeOnly TimingWeeklyTime { get; private set; }
            public int? TimingYearlyDay { get; private set; }
            public int? TimingYearlyMonth { get; private set; }
            public TimeOnly TimingYearlyTime { get; private set; }

            public (bool IsValid, List<TemplateValidationException> ValidationExceptions) ValidateFieldValues( TemplateConfigurationWindow templateWindow )
            {
                TemplateWindow = templateWindow;
                if ( TemplateWindow is null )
                {
                    return ( false, new( ) );
                }

                bool isValid = true;
                string templateName = TemplateWindow.SelectedTemplateItem.TemplateName;
                List<TemplateValidationException> validationExceptions = new( );
                // Naming fields
                NamingComponentSeparator = TemplateWindow.namingComponentSeparatorValidateField.IsValid ? TemplateWindow.namingComponentSeparatorValidateField?.Text?.ToString( ) : null;
                NamingPrefix = TemplateWindow.namingPrefixTextValidateField.IsValid ? TemplateWindow.namingPrefixTextValidateField?.Text?.ToString( ) : null;
                NamingTimestampFormatString = TemplateWindow.namingTimestampFormatTextField?.Text?.ToString( );
                NamingFrequentSuffix = TemplateWindow.namingFrequentSuffixTextValidateField.IsValid ? TemplateWindow.namingFrequentSuffixTextValidateField?.Text?.ToString( ) : null;
                NamingHourlySuffix = TemplateWindow.namingHourlySuffixTextValidateField.IsValid ? TemplateWindow.namingHourlySuffixTextValidateField?.Text?.ToString( ) : null;
                NamingDailySuffix = TemplateWindow.namingDailySuffixTextValidateField.IsValid ? TemplateWindow.namingDailySuffixTextValidateField?.Text?.ToString( ) : null;
                NamingWeeklySuffix = TemplateWindow.namingWeeklySuffixTextValidateField.IsValid ? TemplateWindow.namingWeeklySuffixTextValidateField?.Text?.ToString( ) : null;
                NamingMonthlySuffix = TemplateWindow.namingMonthlySuffixTextValidateField.IsValid ? TemplateWindow.namingMonthlySuffixTextValidateField?.Text?.ToString( ) : null;
                NamingYearlySuffix = TemplateWindow.namingYearlySuffixTextValidateField.IsValid ? TemplateWindow.namingYearlySuffixTextValidateField?.Text?.ToString( ) : null;
                if ( string.IsNullOrWhiteSpace( NamingComponentSeparator ) )
                {
                    string errorMessage = $"Snapshot name component separator value {NamingComponentSeparator} for template {templateName} is invalid";
                    TemplateValidationException ex = new( "Name Component Separator", NamingComponentSeparator, TemplateWindow.namingComponentSeparatorValidateField, errorMessage );
                    validationExceptions.Add( ex );
                    Logger.Warn( ex, errorMessage );
                    isValid = false;
                }

                if ( !TemplateWindow.namingPrefixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingPrefix ) )
                {
                    string errorMessage = $"Snapshot name prefix value {NamingPrefix} for template {templateName} is invalid";
                    TemplateValidationException ex = new( "Name Prefix", NamingPrefix, TemplateWindow.namingPrefixTextValidateField, errorMessage );
                    validationExceptions.Add( ex );
                    Logger.Warn( ex, errorMessage );
                    isValid = false;
                }

                if ( string.IsNullOrWhiteSpace( NamingTimestampFormatString ) )
                {
                    string errorMessage = $"Snapshot timestamp format string value {NamingTimestampFormatString} for template {templateName} is invalid";
                    TemplateValidationException ex = new( "Timestamp Format String", NamingTimestampFormatString, TemplateWindow.namingTimestampFormatTextField, errorMessage );
                    validationExceptions.Add( ex );
                    Logger.Warn( ex, errorMessage );
                    isValid = false;
                }
                else
                {
                    try
                    {
                        string validationTestDateString = DateTimeOffset.Now.ToString( NamingTimestampFormatString );
                        Logger.Debug( "Snapshot temmplate timestamp format string for template {0} is valid and results in output of the form: {1}", templateName, validationTestDateString );
                    }
                    catch ( FormatException ex )
                    {
                        string errorMessage = $"Snapshot timestamp format string value {NamingTimestampFormatString} for template {templateName} is not in the correct format. Please see Microsoft documentation for DateTime format strings.";
                        TemplateValidationException tve = new( "Timestamp Format String", NamingTimestampFormatString, TemplateWindow.namingTimestampFormatTextField, errorMessage, ex );
                        validationExceptions.Add( tve );
                        Logger.Warn( tve, errorMessage );
                        isValid = false;
                    }
                }

                if ( !TemplateWindow.namingFrequentSuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingFrequentSuffix ) )
                {
                    Logger.Warn( "Snapshot template frequent suffix value {0} for template {1} is invalid", NamingFrequentSuffix, templateName );
                    isValid = false;
                }

                if ( !TemplateWindow.namingHourlySuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingHourlySuffix ) )
                {
                    Logger.Warn( "Snapshot template hourly suffix value {0} for template {1} is invalid", NamingHourlySuffix, templateName );
                    isValid = false;
                }

                if ( !TemplateWindow.namingDailySuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingDailySuffix ) )
                {
                    Logger.Warn( "Snapshot template daily suffix value {0} for template {1} is invalid", NamingDailySuffix, templateName );
                    isValid = false;
                }

                if ( !TemplateWindow.namingWeeklySuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingWeeklySuffix ) )
                {
                    Logger.Warn( "Snapshot template weekly suffix value {0} for template {1} is invalid", NamingWeeklySuffix, templateName );
                    isValid = false;
                }

                if ( !TemplateWindow.namingMonthlySuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingMonthlySuffix ) )
                {
                    Logger.Warn( "Snapshot template monthly suffix value {0} for template {1} is invalid", NamingMonthlySuffix, templateName );
                    isValid = false;
                }

                if ( !TemplateWindow.namingYearlySuffixTextValidateField!.IsValid || string.IsNullOrWhiteSpace( NamingYearlySuffix ) )
                {
                    Logger.Warn( "Snapshot template yearly suffix value {0} for template {1} is invalid", NamingYearlySuffix, templateName );
                    isValid = false;
                }

                // Timing fields
                string weeklyDayString = TemplateWindow.timingWeeklyDayTextValidateField.Text.ToString( )!;

                // This exception can't be thrown by the call to string.Equals, because it only happens if the third argument isn't valid
                // ReSharper disable ExceptionNotDocumentedOptional
                int weeklyDayStringIndex = CultureTimeHelpers.DayNamesLongAndAbbreviated.FindIndex( m => string.Equals( m, weeklyDayString, StringComparison.InvariantCultureIgnoreCase ) );
                // ReSharper disable ExceptionNotDocumentedOptional
                DayOfWeek dayOfWeek = (DayOfWeek)( weeklyDayStringIndex % 7 );

                TimingFrequentPeriod = TemplateConfigurationFrequentPeriodOptions[ TemplateWindow.frequentPeriodRadioGroup.SelectedItem ];
                TimingHourlyMinute = TemplateWindow.hourlyMinuteTextValidateField.Text.ToNullableInt32( );
                TimingDailyTime = TimeOnly.FromTimeSpan( TemplateWindow.dailyTimeTimeField.Time );
                TimingWeeklyDay = dayOfWeek;
                TimingWeeklyTime = TimeOnly.FromTimeSpan( TemplateWindow.weeklyTimeTimeField.Time );
                ustring monthlyDayString = TemplateWindow.timingMonthlyDayTextValidateField.Text;
                int monthlyDayInt = monthlyDayString.ToInt32( -1 );
                int numberOfMonthsInYear = CultureInfo.CurrentCulture.Calendar.GetMonthsInYear( DateTimeOffset.Now.Year );
                TimingMonthlyDay = monthlyDayInt;
                TimingMonthlyTime = TimeOnly.FromTimeSpan( TemplateWindow.monthlyTimeTimeField.Time );
                string? enteredYearlyMonthString = TemplateWindow.timingYearlyMonthTextValidateField.Text.ToString( );
                if ( int.TryParse( enteredYearlyMonthString, out int enteredYearlyMonthIntValue ) )
                {
                    TimingYearlyMonth = enteredYearlyMonthIntValue;
                }
                else
                {
                    if ( !string.IsNullOrWhiteSpace( enteredYearlyMonthString ) )
                    {
                        int monthIndex = CultureTimeHelpers.MonthNamesLongAndAbbreviated.FindIndex( m => m.ToLower( CultureInfo.CurrentCulture ) == enteredYearlyMonthString.ToLower( CultureInfo.CurrentCulture ) );
                        if ( monthIndex >= 0 )
                        {
                            TimingYearlyMonth = ( monthIndex % numberOfMonthsInYear ) + 1;
                        }
                    }
                }

                TimingYearlyDay = TemplateWindow.timingYearlyDayTextValidateField?.Text?.ToNullableInt32( );
                TimingYearlyTime = TimeOnly.FromTimeSpan( TemplateWindow.yearlyTimeTimeField.Time );
                if ( TimingFrequentPeriod is null || !TemplateConfigurationFrequentPeriodOptions.Contains( (int)TimingFrequentPeriod ) )
                {
                    Logger.Warn( "Snapshot template frequent period value {0:00} for template {1} is invalid", TimingFrequentPeriod, templateName );
                    isValid = false;
                }

                if ( TimingHourlyMinute is null or < 0 or > 59 )
                {
                    Logger.Warn( "Snapshot template hourly minute value {0:00} for template {1} is invalid", TimingHourlyMinute, templateName );
                    isValid = false;
                }

                if ( TimingWeeklyDay is null )
                {
                    Logger.Warn( "Snapshot template weekly day value {0} for template {1} is invalid", weeklyDayString, templateName );
                    isValid = false;
                }

                if ( TimingMonthlyDay is null )
                {
                    Logger.Warn( "Snapshot template monthly day value {0} for template {1} is invalid", monthlyDayString, templateName );
                    isValid = false;
                }

                if ( TimingYearlyMonth is null )
                {
                    Logger.Warn( "Snapshot template yearly month value {0} for template {1} is invalid", enteredYearlyMonthString, templateName );
                    isValid = false;
                }

                if ( TimingYearlyDay is null )
                {
                    Logger.Warn( "Snapshot template yearly day value {0} for template {1} is invalid", TimingYearlyDay, templateName );
                    isValid = false;
                }

                return ( isValid, validationExceptions );
            }
        }
    }
}
