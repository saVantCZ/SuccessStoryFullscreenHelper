# SuccessStory Fullscreen Helper
Simple Playnite extension which helps expose some data from SuccessStory Plugin to Fullscreen Theme

## If you like my work, a coffee would be awesome!
[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/MtbivzU)

## 📊 Plugin Usage in Theme

Binding via {PluginSettings Plugin=SSHelper, Path=XXX}

| Function         | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `GS15`           | Counts the number of achievements worth **15 GameScore** (Bronze/Common Rarity) |
| `GS30`           | Counts the number of achievements worth **30 GameScore** (Silver/Uncommon Rarity)                |
| `GS90`           | Counts achievements worth **90** and **180 GameScore** (Gold/Rare Rarity)                 |
| `GSPlat`         | Counts how many games have been fully completed (Platinums)                 |
| `GSTotal`        | Total number of achievements unlocked                                       |
| `GSScore`        | Combined GameScore of all unlocked achievements                             |
| `GSLevel`        | Total level calculated based on GameScore                                   |
| `GSLevelProgress`| Percent progress (0–100) toward the next GameScore level                    |
| `GSRank`         | Rank title based on current level: `Bronze1` → `Gold3`, ending with `Plat1` |
| `PlatinumGames`  | Use as ItemSource: Outputs list of Platinum Games in Descending order|
| `PlatinumGamesAscending`| Use as ItemSource: Outputs list of Platinum Games in Ascending order|

---

## 🔧 Requirements

- [SuccessStory Plugin]([https://playnite.link/addons.html#playnite-successstory-plugin])

---

## 📂 File Input Format

Plugin uses JSON data exported by **SuccessStory** plugin. Make sure your achievement data is properly synchronized before using the plugin to ensure accurate results.
