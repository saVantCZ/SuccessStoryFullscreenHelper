# SuccessStory Fullscreen Helper
Simple Playnite extension which helps expose some data from SuccessStory Plugin to Fullscreen Theme

## If you like my work, a coffee would be awesome!
[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/MtbivzU)

## 📊 Plugin Usage in Theme

Add `<ContentControl x:Name="SSHelper_TopBarView"/>` to any Theme to show simple view:


Preview in Playnite default theme:

<img width="284" height="71" alt="image" src="https://github.com/user-attachments/assets/0a6c1eb8-0fb4-4db8-afc4-f5713532c784" />



OR

Use Values in custom element:
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
| `AllGamesWithAchievements`| Use as ItemSource: Outputs list of Separated Games and its Data|




| PlatinumGames Bindings         | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `Name`           | Shows Name of Platinum Game |
| `CoverImagePath`           | Shows its Cover Image              |
| `LatestUnlocked`           | Shows Date when Unlocked                 |

You can also define your Custom Window style:

To Open Achievements window, use This Button Command:
```
Command="{PluginSettings Plugin=SSHelper, Path=OpenAchievementWindow}"
```

Add Behaviours to your resources where style will be defined (So element can get focus)
```
xmlns:pbeh="clr-namespace:Playnite.Behaviors;assembly=Playnite" 
```

Add To your Element which should get focus
```
pbeh:FocusBahaviors.FocusBinding="True"
pbeh:FocusBahaviors.OnVisibilityFocus="True"
```

Achievements Windows Default Style (Theme Makers can customize it)

```xml
<Style x:Key="AchievementsWindowStyle" TargetType="ContentControl">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="ContentControl">
                <Grid >
                    <Grid Margin="40" VerticalAlignment="Top" HorizontalAlignment="Right" Width="400" Height="100">
                        <Border Background="{DynamicResource MainBackgourndBrush}" CornerRadius="10">
                            <StackPanel Orientation="Vertical" HorizontalAlignment="Center"
                                    VerticalAlignment="Center">
                                <StackPanel Orientation="Horizontal" HorizontalAlignment="Center"
                                        VerticalAlignment="Center">
                                    <TextBlock Margin="0,0,0,0" FontFamily="{StaticResource FontIcoFont}" Text="&#xedd6;"
                                            Foreground="#1072d3" FontSize="30" />
                                    <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                            VerticalAlignment="Center" Text="{PluginSettings Plugin=SSHelper, Path=GSPlat}" />
                                    <TextBlock Margin="20,0,0,0" FontFamily="{StaticResource FontIcoFont}"
                                            Text="&#xedd7;" Foreground="#9e7229" FontSize="30" />
                                    <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                            VerticalAlignment="Center"
                                            Text="{PluginSettings Plugin=SSHelper, Path=GS90}" />
                                    <TextBlock Margin="20,0,0,0" FontFamily="{StaticResource FontIcoFont}"
                                            Text="&#xedd7;" Foreground="#5f6366" FontSize="30" />
                                    <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                            VerticalAlignment="Center"
                                            Text="{PluginSettings Plugin=SSHelper, Path=GS30}" />
                                    <TextBlock Margin="20,0,0,0" FontFamily="{StaticResource FontIcoFont}"
                                            Text="&#xedd7;" Foreground="#874233" FontSize="30" />
                                    <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                            VerticalAlignment="Center"
                                            Text="{PluginSettings Plugin=SSHelper, Path=GS15}" />
                                </StackPanel>
                                <Grid>
                                    <StackPanel Orientation="Horizontal" HorizontalAlignment="Left">
                                        <TextBlock Margin="0,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                                VerticalAlignment="Center" Text="Level: " />
                                        <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                                VerticalAlignment="Center"
                                                Text="{PluginSettings Plugin=SSHelper, Path=GSLevel}" />
                                    </StackPanel>
                                    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
                                        <TextBlock Margin="0,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                                VerticalAlignment="Center" Text="Score: " />
                                        <TextBlock Margin="10,0,0,0" Style="{DynamicResource TextBlockBaseStyle}"
                                                VerticalAlignment="Center"
                                                Text="{PluginSettings Plugin=SSHelper, Path=GSScore}" />
                                    </StackPanel>
                                </Grid>
                            </StackPanel>
                        </Border>
                    </Grid>

                    <ListView x:Name="AchievementsList" ItemsSource="{PluginSettings Plugin=SSHelper, Path=AllGamesWithAchievements}" VerticalAlignment="Center" HorizontalAlignment="Center"
                            VirtualizingPanel.VirtualizationMode="Recycling" VirtualizingPanel.IsVirtualizing="True"
                            pbeh:FocusBahaviors.FocusBinding="True" pbeh:FocusBahaviors.OnVisibilityFocus="True"
                            Background="Transparent" BorderBrush="Transparent" Height="850" BorderThickness="0" FocusVisualStyle="{x:Null}" Focusable="True">
                        <ListView.ItemContainerStyle>
                            <Style TargetType="ListViewItem">
                                <Setter Property="FocusVisualStyle" Value="{x:Null}" />
                                <Setter Property="Focusable" Value="True" />
                                <Setter Property="Template">
                                    <Setter.Value>
                                        <ControlTemplate TargetType="ListViewItem">
                                            <Grid Margin="10" Width="700" Height="130"  VerticalAlignment="Top">
                                                <Border Background="{DynamicResource ControlBackgroundDarkBrush}"
                                                        CornerRadius="3">
                                                    <Grid>

                                                        <StackPanel Orientation="Horizontal"
                                                                VerticalAlignment="Center"
                                                                HorizontalAlignment="Left">
                                                            <StackPanel Orientation="Vertical" Margin="0,0,0,0">
                                                                <Image Width="80" Height="80" Margin="10,10,0,0"
                                                                        VerticalAlignment="Top"
                                                                        HorizontalAlignment="Left"
                                                                        Source="{Binding CoverImagePath}" />
                                                                <TextBlock Margin="10,0,0,5"
                                                                        HorizontalAlignment="Left"
                                                                        Style="{DynamicResource TextBlockBaseStyle}"
                                                                        VerticalAlignment="Bottom"
                                                                        Text="{Binding Name}" />
                                                            </StackPanel>
                                                            
                                                            
                                                        </StackPanel>

                                                        <StackPanel Margin="100,0,0,0" Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Left">
                                                            <TextBlock Margin="30,0,0,0"
                                                                    Style="{DynamicResource TextBlockBaseStyle}"
                                                                    VerticalAlignment="Center" Text="Progress:" />
                                                            <TextBlock Margin="5,0,0,0"
                                                                    Style="{DynamicResource TextBlockBaseStyle}"
                                                                    VerticalAlignment="Center"
                                                                    Text="{Binding Progress, StringFormat={}{0}%}" />
                                                        </StackPanel>

                                                        <StackPanel Orientation="Horizontal" Margin="0,0,10,0"
                                                                HorizontalAlignment="Right"
                                                                VerticalAlignment="Center">
                                                            <Grid x:Name="Plat" Visibility="Collapsed">
                                                                <TextBlock Margin="0,0,5,0"
                                                                        FontFamily="{StaticResource FontIcoFont}"
                                                                        Text="&#xedd6;" Foreground="#1072d3"
                                                                        FontSize="30" />
                                                                <TextBlock Margin="10,0,0,0"
                                                                        Style="{DynamicResource TextBlockBaseStyle}"
                                                                        VerticalAlignment="Center"
                                                                        Text="1" />
                                                            </Grid>
                                                            <TextBlock Margin="20,0,0,0"
                                                                    FontFamily="{StaticResource FontIcoFont}"
                                                                    Text="&#xedd7;" Foreground="#9e7229"
                                                                    FontSize="30" />
                                                            <TextBlock Margin="10,0,0,0"
                                                                    Style="{DynamicResource TextBlockBaseStyle}"
                                                                    VerticalAlignment="Center"
                                                                    Text="{Binding GS90Count}" />
                                                            <TextBlock Margin="20,0,0,0"
                                                                    FontFamily="{StaticResource FontIcoFont}"
                                                                    Text="&#xedd7;" Foreground="#5f6366"
                                                                    FontSize="30" />
                                                            <TextBlock Margin="10,0,0,0"
                                                                    Style="{DynamicResource TextBlockBaseStyle}"
                                                                    VerticalAlignment="Center"
                                                                    Text="{Binding GS30Count}" />
                                                            <TextBlock Margin="20,0,0,0"
                                                                    FontFamily="{StaticResource FontIcoFont}"
                                                                    Text="&#xedd7;" Foreground="#874233"
                                                                    FontSize="30" />
                                                            <TextBlock Margin="10,0,0,0"
                                                                    Style="{DynamicResource TextBlockBaseStyle}"
                                                                    VerticalAlignment="Center"
                                                                    Text="{Binding GS15Count}" />
                                                        </StackPanel>
                                                        <Border CornerRadius="3" BorderThickness="3"
                                                                BorderBrush="{DynamicResource ControlBackgroundBrush}"
                                                                Margin="-3" />
                                                        <Border x:Name="MainBorder" BorderThickness="5"
                                                                BorderBrush="{DynamicResource SelectionBrush}"
                                                                Margin="-5" Visibility="Collapsed" CornerRadius="3">
                                                            <Border.Effect>
                                                                <DropShadowEffect ShadowDepth="0" Opacity="1"
                                                                        Color="{DynamicResource GlyphColor}"
                                                                        RenderingBias="Quality" BlurRadius="20" />
                                                            </Border.Effect>
                                                        </Border>

                                                    </Grid>
                                                </Border>
                                            </Grid>
                                            <ControlTemplate.Triggers>
                                                <Trigger Property="IsFocused" Value="True">
                                                    <Setter TargetName="MainBorder" Property="Visibility"
                                                            Value="Visible" />
                                                </Trigger>
                                                <DataTrigger Binding="{Binding IsPlatinum}" Value="True">
                                                    <Setter TargetName="Plat" Property="Visibility" Value="Visible" />
                                                </DataTrigger>
                                            </ControlTemplate.Triggers>
                                        </ControlTemplate>
                                    </Setter.Value>
                                </Setter>
                            </Style>
                        </ListView.ItemContainerStyle>
                    </ListView>
                    
                    
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>

```

| AllGamesWithAchievements Bindings         | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `Name`           | Shows Name of Game |
| `CoverImagePath`           | Shows its Cover Image              |
| `GS15Count`           | Count of Bronze Achievements               |
| `GS30Count`           | Count of Silver Achievements               |
| `GS80Count`           | Count of Gold Achievements                |
| `Progress`           | Progress from 0-100 of unlocking achievements               |
| `IsPlatinum`           | True/False if its Platinum                |


---

## 🔧 Requirements

- [SuccessStory Plugin]([https://playnite.link/addons.html#playnite-successstory-plugin])

---

## 📂 File Input Format

Plugin uses JSON data exported by **SuccessStory** plugin. Make sure your achievement data is properly synchronized before using the plugin to ensure accurate results.
