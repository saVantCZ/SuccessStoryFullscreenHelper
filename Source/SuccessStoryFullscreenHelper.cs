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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace SuccessStoryFullscreenHelper
{
    public class SuccessStoryFullscreenHelper : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public static SuccessStoryFullscreenHelper Instance { get; private set; }
        public static SuccessStoryFullscreenHelperSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("fd098238-28e4-42a8-a313-712dc2834237");

        public SuccessStoryFullscreenHelper(IPlayniteAPI api) : base(api)
        {
            settings = new SuccessStoryFullscreenHelperSettingsViewModel(this);
            Instance = this;
            Properties = new GenericPluginProperties
            {
                HasSettings = false
            };
            AddSettingsSupport(new AddSettingsSupportArgs
            {
                SourceName = "SSHelper",
                SettingsRoot = $"settings.Settings"
            });
            AddCustomElementSupport(new AddCustomElementSupportArgs
            {
                ElementList = new List<string> { "TopBarView" },
                SourceName = "SSHelper"
            });
        }



        public class PlatinumGame
        {
            public string Name { get; set; }
            public Guid GameId { get; set; }
            public string CoverImagePath { get; set; }
            public DateTime LatestUnlocked { get; set; }
            public ICommand OpenAchievementWindow { get; set; }
        }

        public class GameAchievementsData
        {
            public ICommand OpenAchievementWindow { get; set; }
            public string Name { get; set; }
            public Guid GameId { get; set; }
            public string CoverImagePath { get; set; }
            public int GS15Count { get; set; }
            public int GS30Count { get; set; }
            public int GS90Count { get; set; }
            public int Progress { get; set; }
            public bool IsPlatinum { get; set; }
            public DateTime LastUnlockDate { get; set; }
        }

        private Window AchievementsWindow;
        public void ShowAchievementsWindow(IPlayniteAPI api, string styleName = "AchievementsWindowStyle")
        {
            if (AchievementsWindow != null && AchievementsWindow.IsVisible)
            {
                AchievementsWindow.Close(); // close old window before opening new one
            }
            AchievementsWindow = api.Dialogs.CreateWindow(new WindowCreationOptions
            {
                ShowMinimizeButton = false,
                
            });

            var parent = api.Dialogs.GetCurrentAppWindow();

            AchievementsWindow.Owner = parent; 

            AchievementsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            AchievementsWindow.Title = "Achievements";

            AchievementsWindow.Height = parent.Height;
            AchievementsWindow.Width = parent.Width;

            string xamlString = $@"
            <Viewbox Stretch=""Uniform"" 
                     xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                     xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                     xmlns:pbeh=""clr-namespace:Playnite.Behaviors;assembly=Playnite"">
                <Grid Width=""1920"" Height=""1080"">
                    <ContentControl x:Name=""AchievementsWindow""
                                    Focusable=""False""
                                    Style=""{{DynamicResource {styleName}}}"" />
                </Grid>
            </Viewbox>";

            var content = (System.Windows.FrameworkElement)System.Windows.Markup.XamlReader.Parse(xamlString);

            AchievementsWindow.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    AchievementsWindow.Close();
                    e.Handled = true;
                }
            };

            AchievementsWindow.Content = content;
            AchievementsWindow.ShowDialog();

        }

        public override void OnControllerButtonStateChanged(OnControllerButtonStateChangedArgs args)
        {
            if (args.Button == ControllerInput.B && args.State == ControllerInputState.Pressed)
            {
                if (AchievementsWindow != null && AchievementsWindow.IsVisible)
                {
                    AchievementsWindow.Close();
                }
            }
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
            var platinumGamesList = new List<PlatinumGame>();
            var allGamesWithAchievements = new List<GameAchievementsData>();


            foreach (var file in Directory.EnumerateFiles(dataPath, "*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    fileCount++;
                    string content = File.ReadAllText(file);
                    dynamic json = Serialization.FromJson<dynamic>(content);

                    if (json.Items != null && json.Items.Count > 0)
                    {
                        bool allUnlocked = true;

                        foreach (var item in json.Items)
                        {
                            if (item["DateUnlocked"] == null ||
                                item["DateUnlocked"].ToString().StartsWith("0001-01-01"))
                            {
                                allUnlocked = false;
                                break;
                            }
                        }

                        if (allUnlocked)
                        {
                            string gameName = json["Name"]?.ToString() ?? "Unknown Game";

                            DateTime latestUnlocked = DateTime.MinValue;

                            foreach (var item in json.Items)
                            {
                                if (item["DateUnlocked"] != null &&
                                    !item["DateUnlocked"].ToString().StartsWith("0001-01-01"))
                                {
                                    if (DateTime.TryParse(item["DateUnlocked"].ToString(), out DateTime unlockedDate))
                                    {
                                        if (unlockedDate > latestUnlocked)
                                        {
                                            latestUnlocked = unlockedDate;
                                        }
                                    }
                                }
                            }

                            if (latestUnlocked > DateTime.MinValue)
                            {
                                gsPlat++;
                                var matchedGame = PlayniteApi.Database.Games.FirstOrDefault(g => g.Name == gameName);
                                string coverPath = null;

                                if (matchedGame != null && !string.IsNullOrEmpty(matchedGame.CoverImage))
                                {
                                    try
                                    {
                                        coverPath = PlayniteApi.Database.GetFullFilePath(matchedGame.CoverImage);
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Warn($"Failed to get cover image for {gameName}: {ex.Message}");
                                    }
                                }

                                var gameGuid = new Guid(Path.GetFileNameWithoutExtension(file));

                                platinumGamesList.Add(new PlatinumGame
                                {
                                    Name = gameName,
                                    GameId = gameGuid,
                                    CoverImagePath = coverPath,
                                    LatestUnlocked = latestUnlocked,
                                    OpenAchievementWindow = new RelayCommand(() =>
                                    {
                                        PlayniteApi.MainView.SelectGame(gameGuid);
                                        ShowAchievementsWindow(PlayniteApi, "GameAchievementsWindowStyle");
                                    })
                                });
                            }
                            else
                            {
                                logger.Warn($"Game {gameName} has no valid LatestUnlocked date, skipping adding to platinumGamesList.");
                            }
                        }

                        string platformName = json.SourcesLink?.Name?.ToString() ?? "";

                        var retroPlatforms = new HashSet<string>
                        {
                            "RetroAchievements"
                        };


                        foreach (var item in json.Items)
                        {
                            if (item["DateUnlocked"] != null &&
                                !item["DateUnlocked"].ToString().StartsWith("0001-01-01"))
                            {
                                double score = (double)item["GamerScore"];

                                // Speciální pravidla pro RetroAchievements
                                if (retroPlatforms.Contains(platformName))
                                {
                                    if (score >= 1 && score <= 9)
                                        gs15++;
                                    else if (score >= 10 && score <= 19)
                                        gs30++;
                                    else if (score >= 20 && score <= 50)
                                        gs90++;
                                }
                                else
                                {
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
                    if (json.Items != null && json.Items.Count > 0)
                    {
                        int gameGS15 = 0;
                        int gameGS30 = 0;
                        int gameGS90 = 0;
                        int unlockedCount = 0;
                        int totalCount = json.Items.Count;
                        DateTime latestUnlockDate = DateTime.MinValue;

                        string platformName = json.SourcesLink?.Name?.ToString() ?? "";

                        var retroPlatforms = new HashSet<string>
                        {
                            "RetroAchievements"
                        };

                        foreach (var item in json.Items)
                        {
                            bool unlocked = item["DateUnlocked"] != null &&
                                            !item["DateUnlocked"].ToString().StartsWith("0001-01-01");

                            if (unlocked)
                            {
                                unlockedCount++;

                                if (DateTime.TryParse(item["DateUnlocked"].ToString(), out DateTime unlockedDate))
                                {
                                    if (unlockedDate > latestUnlockDate)
                                        latestUnlockDate = unlockedDate;
                                }

                                double score = 0;
                                try
                                {
                                    score = (double)item["GamerScore"];
                                }
                                catch { /* ignorovat */ }

                                if (retroPlatforms.Contains(platformName))
                                {
                                    // Speciální pravidla pro RetroAchievements
                                    if (score >= 1 && score <= 9) 
                                        gameGS15++;
                                    else if (score >= 10 && score <= 19) 
                                        gameGS30++;
                                    else if (score >= 20 && score <= 50) 
                                        gameGS90++;
                                }
                                else
                                {
                                    if (score == 15.0) 
                                        gameGS15++;
                                    else if (score == 30.0) 
                                        gameGS30++;
                                    else if (score == 90.0 || score == 180.0) 
                                        gameGS90++;
                                }
                            }
                        }

                        int progressPercent = totalCount > 0 ? (int)(100.0 * unlockedCount / totalCount) : 0;
                        bool isPlatinum = progressPercent == 100;

                        string gameName = json["Name"]?.ToString() ?? "Unknown Game";

                        var matchedGame = PlayniteApi.Database.Games.FirstOrDefault(g => g.Name == gameName);
                        string coverPath = null;
                        if (matchedGame != null && !string.IsNullOrEmpty(matchedGame.CoverImage))
                        {
                            try
                            {
                                coverPath = PlayniteApi.Database.GetFullFilePath(matchedGame.CoverImage);
                            }
                            catch (Exception ex)
                            {
                                logger.Warn($"Failed to get cover image for {gameName}: {ex.Message}");
                            }
                        }

                        var gameGuid = new Guid(Path.GetFileNameWithoutExtension(file));

                        allGamesWithAchievements.Add(new GameAchievementsData
                        {
                            Name = gameName,
                            GameId = gameGuid,
                            CoverImagePath = coverPath,
                            GS15Count = gameGS15,
                            GS30Count = gameGS30,
                            GS90Count = gameGS90,
                            Progress = progressPercent,
                            IsPlatinum = isPlatinum,
                            LastUnlockDate = latestUnlockDate,
                            OpenAchievementWindow = new RelayCommand(() =>
                            {
                                PlayniteApi.MainView.SelectGame(gameGuid);
                                ShowAchievementsWindow(PlayniteApi, "GameAchievementsWindowStyle");
                            })
                        });
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
            int scorePlat = gsPlat * 300;
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

            if (level <= 3)
                rank = "Bronze1";
            else if (level <= 7)
                rank = "Bronze2";
            else if (level <= 12)
                rank = "Bronze3";
            else if (level <= 21)
                rank = "Silver1";
            else if (level <= 31)
                rank = "Silver2";
            else if (level <= 44)
                rank = "Silver3";
            else if (level <= 59)
                rank = "Gold1";
            else if (level <= 77)
                rank = "Gold2";
            else if (level <= 97)
                rank = "Gold3";
            else if (level <= 119)
                rank = "Plat1";
            else if (level <= 144)
                rank = "Plat2";
            else if (level <= 171)
                rank = "Plat3";
            else
                rank = "Plat";

            platinumGamesList = platinumGamesList
                .OrderByDescending(p => p.LatestUnlocked)
                .ToList();

            var platinumGamesListAscending = platinumGamesList
                .OrderBy(p => p.LatestUnlocked)
                .ToList();



            settings.Settings.GS15 = gs15.ToString();
            settings.Settings.GS30 = gs30.ToString();
            settings.Settings.GS90 = gs90.ToString();
            settings.Settings.GSTotal = total.ToString();
            settings.Settings.GSScore = combinedScore.ToString("N0");
            settings.Settings.GSLevel = level.ToString();
            settings.Settings.GSLevelProgress = progress.ToString();
            settings.Settings.GSPlat = gsPlat.ToString();
            settings.Settings.GSRank = rank.ToString();
            settings.Settings.PlatinumGames = platinumGamesList;
            settings.Settings.PlatinumGamesAscending = platinumGamesListAscending;
            settings.Settings.AllGamesWithAchievements = allGamesWithAchievements
            .OrderByDescending(g => g.LastUnlockDate)
            .ToList();

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
        public override Control GetGameViewControl(GetGameViewControlArgs args)
        {
            if (args.Name == "TopBarView")
            {
                return new TopBarView(settings);
            }

            return null;
        }
    }
}