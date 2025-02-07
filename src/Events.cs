using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;

namespace Sutol;

public partial class Main
{
    public HookResult OnWeaponCanUse(DynamicHook hook)
    {
        var clientweapon = hook.GetParam<CBasePlayerWeapon>(1);
        var player = clientweapon.OwnerEntity.Value?.As<CCSPlayerController>();
        if (sutList.Contains(player))
        {
            hook.SetReturn(false);
            return HookResult.Stop;
        }

        return HookResult.Continue;
    }

    public HookResult TakeDamageOld(DynamicHook hook)
    {
        var info = hook.GetParam<CTakeDamageInfo>(1);

        if (info.Attacker.Value?.DesignerName != "player" || info.Attacker.Value == null)
        {
            return HookResult.Continue;
        }

        var player = info.Attacker.Value.As<CCSPlayerController>();

        if (sutList.Contains(player))
        {
            hook.SetReturn(false);
            return HookResult.Stop;
        }

        return HookResult.Continue;
    }

    public static HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        var player = @event.Userid;
        player.sutBoz();
        return HookResult.Continue;
    }

    public static HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player.sutBoz())
        {
            Server.PrintToChatAll(
                $"{Prefix} {ChatColors.Red}{player?.PlayerName} {ChatColors.White}Adlı oyuncu öldüğü için sütlükten atılmıştır!!");
        }

        return HookResult.Continue;
    }

    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        var sutPlayers = sutList;
        if (sutPlayers.Count > 0)
        {
            foreach (var player in sutPlayers)
            {
                player.sutBoz();
            }
        }

        SutAktif = true;
        return HookResult.Continue;
    }


    public void PrecacheMonitfests(ResourceManifest manifest)
    {
        if (Config.SutModeli != null)
        {
            manifest.AddResource(Config.SutModeli);
        }
    }

    public void OnTick()
    {
        if (sutList.Count == 0 || !Config.SutGokkusagi)
            return;

        foreach (var player in sutList)
        {
            if (!player.IsValid()) continue;

            if (!PreviousColors.ContainsKey(player))
            {
                PreviousColors[player] = Schema.GetSchemaValue<Color>(player.PlayerPawn.Value.Handle,
                    "CBaseModelEntity", "m_clrRender");
            }

            Color newColor = Helpers.GetNextRainbowColor(player);
            player.SetColour(newColor);
        }
    }

    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        Server.PrintToChatAll(
            $"{Prefix} {ChatColors.White}Süt olma süresi {Config.SutSuresi} saniye sonra sona erecektir.");
        AddTimer(Config.SutSuresi, () =>
        {
            SutAktif = false;
            Server.PrintToChatAll($"{Prefix} {ChatColors.White}Süt alımı kapatılmıştır.");
        });
        return HookResult.Continue;
    }
}
