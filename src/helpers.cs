using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using static Sutol.Main;

public static class Helpers
{
    public static bool IsValid(this CCSPlayerController player)
    {
        if (player != null && player.PlayerPawn.Value != null && player.PawnIsAlive) return true;
        return false;
    }

    public static bool sutBoz(this CCSPlayerController? player)
    {
        if (Instance.sutList!.Contains(player))
        {
            if (player.Connected == PlayerConnectedState.PlayerConnected &&
                Instance.playerOldModels.ContainsKey(player))
            {
                setOldModel(player);
            }

            if (Instance.PreviousColors.ContainsKey(player))
            {
                Instance.PreviousColors.Remove(player);
            }

            Instance.sutList.Remove(player);
            player.SetColour(Color.FromArgb(255, 255, 255, 255));
            return true;
        }

        return false;
    }

    public static void setOldModel(this CCSPlayerController player)
    {
        player.PlayerPawn.Value.SetModel(Instance.playerOldModels[player]);
    }

    public static bool sutOl(this CCSPlayerController? player)
    {
        if (!Instance.sutList.Contains(player))
        {
            if (player.Connected == PlayerConnectedState.PlayerConnected &&
                !Instance.playerOldModels.ContainsKey(player) && Instance.Config.SutModeli != null)
            {
                Instance.playerOldModels.Add(player, player.getCurrentPlayerModel());
                player.PlayerPawn.Value?.SetModel(Instance.Config.SutModeli);
            }

            Instance.sutList.Add(player);
            return true;
        }

        return false;
    }

    public static void SetColour(this CCSPlayerController? player, Color colour)
    {
        CCSPlayerPawn pawn = player?.PlayerPawn.Value;
        if (pawn != null && player.IsValid())
        {
            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            pawn.Render = colour;
            Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
        }
    }


    public static Color GetNextRainbowColor(CCSPlayerController player)
    {
        Color previousColor = Instance.PreviousColors.ContainsKey(player) ? Instance.PreviousColors[player] : Color.Red;

        float hueShift = 10f;
        float hue, saturation, brightness;
        ColorToHSV(previousColor, out hue, out saturation, out brightness);
        hue = (hue + hueShift) % 360;

        Color nextColor = ColorFromHSV(hue, saturation, brightness);
        Instance.PreviousColors[player] = nextColor;
        return nextColor;
    }

    public static void ColorToHSV(Color color, out float hue, out float saturation, out float brightness)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));
        float delta = max - min;

        hue = 0f;
        if (delta > 0f)
        {
            if (max == r)
                hue = 60f * (((g - b) / delta) % 6);
            else if (max == g)
                hue = 60f * (((b - r) / delta) + 2);
            else if (max == b)
                hue = 60f * (((r - g) / delta) + 4);
        }

        if (hue < 0) hue += 360f;

        saturation = max == 0 ? 0 : (delta / max);
        brightness = max;
    }

    public static Color ColorFromHSV(float hue, float saturation, float brightness)
    {
        float c = brightness * saturation;
        float x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        float m = brightness - c;

        float r = 0, g = 0, b = 0;
        if (hue < 60)
        {
            r = c;
            g = x;
        }
        else if (hue < 120)
        {
            r = x;
            g = c;
        }
        else if (hue < 180)
        {
            g = c;
            b = x;
        }
        else if (hue < 240)
        {
            g = x;
            b = c;
        }
        else if (hue < 300)
        {
            r = x;
            b = c;
        }
        else
        {
            r = c;
            b = x;
        }

        return Color.FromArgb(
            (int)((r + m) * 255),
            (int)((g + m) * 255),
            (int)((b + m) * 255)
        );
    }

    public static string getCurrentPlayerModel(this CCSPlayerController? player)
    {
        return player.PlayerPawn.Value.CBodyComponent.SceneNode?.GetSkeletonInstance().ModelState.ModelName ?? null;
    }
}