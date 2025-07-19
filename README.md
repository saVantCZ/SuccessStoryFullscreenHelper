# SuccessStory Fullscreen Helper
Simple Playnite extension which helps expose some data from SuccessStory Plugin to Fullscreen Theme

## 📊 Plugin Usage in Theme

Binding via {PluginSettings Plugin=SSHelper, Path=XXX}

| Function         | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `GS15`           | Counts the number of achievements worth **15 GameScore**                    |
| `GS30`           | Counts the number of achievements worth **30 GameScore**                    |
| `GS90`           | Counts achievements worth **90** and **180 GameScore**                      |
| `GSPlat`         | Counts how many games have been fully completed (Platinums)                 |
| `GSTotal`        | Total number of achievements unlocked                                       |
| `GSScore`        | Combined GameScore of all unlocked achievements                             |
| `GSLevel`        | Total level calculated based on GameScore                                   |
| `GSLevelProgress`| Percent progress (0–100) toward the next GameScore level                    |
| `GSRank`         | Rank title based on current level: `Bronze1` → `Gold3`, ending with `Plat1` |

---

## 🔧 Requirements

- [SuccessStory Plugin]([https://playnite.link/addons.html#playnite-successstory-plugin])

---

## 📂 File Input Format

Plugin uses JSON data exported by **SuccessStory** plugin. Make sure your achievement data is properly synchronized before using the plugin to ensure accurate results.
