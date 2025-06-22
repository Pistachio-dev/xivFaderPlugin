using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FaderPlugin.Config;
using System;

namespace FaderPlugin;

public class UIVisibilityControl : IDalamudPlugin
{
    public bool ShowingHUD = true;

    [PluginService] public static IDalamudPluginInterface PluginInterface { get; set; } = null!;
    [PluginService] public static IKeyState KeyState { get; set; } = null!;
    [PluginService] public static IFramework Framework { get; set; } = null!;
    [PluginService] public static IClientState ClientState { get; set; } = null!;
    [PluginService] public static ICondition Condition { get; set; } = null!;
    [PluginService] public static ICommandManager CommandManager { get; set; } = null!;
    [PluginService] public static IChatGui ChatGui { get; set; } = null!;
    [PluginService] public static IGameGui GameGui { get; set; } = null!;
    [PluginService] public static ITargetManager TargetManager { get; set; } = null!;
    [PluginService] public static IDataManager Data { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;

    public UIVisibilityControl()
    {
    }

    public void Dispose()
    {
        UpdateAddonVisibility(true);
    }

    public void UpdateAddonVisibility(bool visible)
    {
        if (!IsSafeToWork())
            return;

        foreach (Element element in Enum.GetValues(typeof(Element)))
        {
            string[] addonNames = ElementUtil.GetAddonName(element);

            if (addonNames.Length == 0)
            {
                continue;
            }

            foreach (string addonName in addonNames)
            {
                Addon.SetAddonVisibility(addonName, visible);
            }

            ShowingHUD = visible;
        }
    }

    /// <summary>
    /// Returns whether it is safe for the plugin to perform work,
    /// dependent on whether the game is on a login or loading screen.
    /// </summary>
    private bool IsSafeToWork()
    {
        return !Condition[ConditionFlag.BetweenAreas] && ClientState.IsLoggedIn;
    }
}
