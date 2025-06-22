using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;

namespace FaderPlugin;

public static unsafe class Addon
{
    private static readonly AtkStage* stage = AtkStage.Instance();

    private static readonly Dictionary<string, (short, short)> StoredPositions = new();

    public static void SetAddonVisibility(string name, bool isVisible, IGameGui gameGui)
    {
        nint addonPointer = gameGui.GetAddonByName(name, 1);
        if (addonPointer == nint.Zero)
        {
            return;
        }

        AtkUnitBase* addon = (AtkUnitBase*)addonPointer;

        if (isVisible)
        {
            // Restore the elements position on screen.
            if (StoredPositions.TryGetValue(name, out var position) && (addon->X == -9999 || addon->Y == -9999))
            {
                var (x, y) = position;
                addon->SetPosition(x, y);
            }
        }
        else
        {
            // Store the position prior to hiding the element.
            if (addon->X != -9999 && addon->Y != -9999)
            {
                StoredPositions[name] = (addon->X, addon->Y);
            }

            // Move the element off screen so it can't be interacted with.
            addon->SetPosition(-9999, -9999);
        }
    }
}
