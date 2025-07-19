using Playnite.SDK;
using Playnite.SDK.Data;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SuccessStoryFullscreenHelper
{
    public class SuccessStoryFullscreenHelper : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public SuccessStoryFullscreenHelperSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("fd098238-28e4-42a8-a313-712dc2834237");

        public SuccessStoryFullscreenHelper(IPlayniteAPI api) : base(api)
        {
            settings = new SuccessStoryFullscreenHelperSettingsViewModel(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = false
            };
            AddSettingsSupport(new AddSettingsSupportArgs
            {
                SourceName = "SSHelper",
                SettingsRoot = $"settings.Settings"
            });
        }

        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(OnGameStartedEventArgs args)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(OnGameStartingEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            base.OnApplicationStarted(args);
            CountAchievements();
        }

        private void CountAchievements()
        {
            string dataPath = Path.Combine(PlayniteApi.Paths.ExtensionsDataPath, "cebe6d32-8c46-4459-b993-5a5189d60788", "SuccessStory");

            if (!Directory.Exists(dataPath))
            {
                logger.Warn($"SuccessStory folder not found at: {dataPath}");
                return;
            }

            int gs15 = 0;
            int gs30 = 0;
            int gs90 = 0;
            int gsPlat = 0;
            int fileCount = 0;


            foreach (var file in Directory.EnumerateFiles(dataPath, "*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    fileCount++;
                    string content = File.ReadAllText(file);
                    dynamic json = Serialization.FromJson<dynamic>(content);

                    if (json.Items != null)
                    {
                        bool allUnlocked = true;

                        foreach (var item in json.Items)
                        {
                            if (item["DateUnlocked"] == null)
                            {
                                allUnlocked = false;
                                break;
                            }
                        }

                        if (allUnlocked && json.Items.Count > 0)
                        {
                            gsPlat++;
                        }

                        foreach (var item in json.Items)
                        {
                            if (item["DateUnlocked"] != null)
                            {
                                double score = (double)item["GamerScore"];

                                if (score == 15.0)
                                    gs15++;
                                else if (score == 30.0)
                                    gs30++;
                                else if (score == 90.0 || score == 180.0)
                                    gs90++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Failed to process file: {file}");
                }
            }

            int score15 = gs15 * 15;
            int score30 = gs30 * 30;
            int score90 = gs90 * 90;
            int scorePlat = gsPlat * 180;
            int combinedScore = score15 + score30 + score90 + scorePlat ;
            int level = 0;
            int rangeMin = 0;
            int rangeMax = 100;
            int step = 100;
            int total = gs15 + gs30 + gs90 + gsPlat;

            while (combinedScore > rangeMax)
            {
                level++;
                rangeMin = rangeMax + 1;
                step += 100;
                rangeMax = rangeMin + step - 1;
            }

            int rangeSpan = rangeMax - rangeMin + 1;
            int progress = (int)(((double)(combinedScore - rangeMin) / rangeSpan) * 100);
            progress = Math.Max(0, Math.Min(100, progress));

            string rank;

            if (level <= 4)
                rank = "Bronze1";
            else if (level <= 9)
                rank = "Bronze2";
            else if (level <= 14)
                rank = "Bronze3";
            else if (level <= 19)
                rank = "Silver1";
            else if (level <= 24)
                rank = "Silver2";
            else if (level <= 29)
                rank = "Silver3";
            else if (level <= 34)
                rank = "Gold1";
            else if (level <= 39)
                rank = "Gold2";
            else if (level <= 44)
                rank = "Gold3";
            else
                rank = "Plat1";

            settings.Settings.GS15 = gs15.ToString();
            settings.Settings.GS30 = gs30.ToString();
            settings.Settings.GS90 = gs90.ToString();
            settings.Settings.GSTotal = total.ToString();
            settings.Settings.GSScore = combinedScore.ToString("N0");
            settings.Settings.GSLevel = level.ToString();
            settings.Settings.GSLevelProgress = progress.ToString();
            settings.Settings.GSPlat = gsPlat.ToString();
            settings.Settings.GSRank = rank.ToString();

            logger.Info($"SuccessStory stats loaded from {fileCount} files. Bronze: {gs15}, Silver: {gs30}, Gold: {gs90}, Platinum: {gsPlat}, Total: {total}");
        }



        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new SuccessStoryFullscreenHelperSettingsView();
        }
    }
}