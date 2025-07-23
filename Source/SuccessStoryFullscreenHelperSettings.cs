using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuccessStoryFullscreenHelper.SuccessStoryFullscreenHelper;

namespace SuccessStoryFullscreenHelper
{
    public class SuccessStoryFullscreenHelperSettings : ObservableObject
    {
        private string gs15 = "0";
        private string gs30 = "0";
        private string gs90 = "0";
        private string gsPlat = "0";
        private string gsTotal = "0";
        private string gsScore = "0";
        private string gsLevel = "0";
        private string gsLevelProgress = "0";
        private string gsRank = "Bronze1";

        private List<PlatinumGame> _platinumGames = new List<PlatinumGame>();
        public List<PlatinumGame> PlatinumGames
        {
            get => _platinumGames;
            set => SetValue(ref _platinumGames, value);
        }

        private List<PlatinumGame> _platinumGamesAscending = new List<PlatinumGame>();
        public List<PlatinumGame> PlatinumGamesAscending
        {
            get => _platinumGamesAscending;
            set => SetValue(ref _platinumGamesAscending, value);
        }

        public string GS15 { get => gs15; set => SetValue(ref gs15, value); }
        public string GS30 { get => gs30; set => SetValue(ref gs30, value); }
        public string GS90 { get => gs90; set => SetValue(ref gs90, value); }
        public string GSPlat { get => gsPlat; set => SetValue(ref gsPlat, value); }
        public string GSTotal { get => gsTotal; set => SetValue(ref gsTotal, value); }
        public string GSScore { get => gsScore; set => SetValue(ref gsScore, value); }
        public string GSLevel { get => gsLevel; set => SetValue(ref gsLevel, value); }
        public string GSLevelProgress { get => gsLevelProgress; set => SetValue(ref gsLevelProgress, value); }
        public string GSRank { get => gsRank; set => SetValue(ref gsRank, value); }


        private string option1 = string.Empty;
        private bool option2 = false;
        private bool optionThatWontBeSaved = false;

        public string Option1 { get => option1; set => SetValue(ref option1, value); }
        public bool Option2 { get => option2; set => SetValue(ref option2, value); }
        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        [DontSerialize]
        public bool OptionThatWontBeSaved { get => optionThatWontBeSaved; set => SetValue(ref optionThatWontBeSaved, value); }
    }

    public class SuccessStoryFullscreenHelperSettingsViewModel : ObservableObject, ISettings
    {
        private readonly SuccessStoryFullscreenHelper plugin;
        private SuccessStoryFullscreenHelperSettings editingClone { get; set; }

        private SuccessStoryFullscreenHelperSettings settings;
        public SuccessStoryFullscreenHelperSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        public SuccessStoryFullscreenHelperSettingsViewModel(SuccessStoryFullscreenHelper plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<SuccessStoryFullscreenHelperSettings>();

            // LoadPluginSettings returns null if no saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new SuccessStoryFullscreenHelperSettings();
            }
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.
            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }
}