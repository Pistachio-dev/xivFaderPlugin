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
    private readonly ICondition condition;
    private readonly IClientState clientState;
    private readonly IGameGui gameGui;
    public bool ShowingHUD = true;

    public UIVisibilityControl(ICondition condition, IClientState clientState, IGameGui gameGui)
    {
        this.condition = condition;
        this.clientState = clientState;
        this.gameGui = gameGui;
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
                Addon.SetAddonVisibility(addonName, visible, gameGui);
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
        return !condition[ConditionFlag.BetweenAreas]
            && clientState.IsLoggedIn;
    }
}
