using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using static Helpers;
using static Sutol.Main;

public static class Events
{
    public static HookResult OnWeaponCanUse(DynamicHook hook)
    {
        var clientweapon = hook.GetParam<CBasePlayerWeapon>(1);
        var player = clientweapon.OwnerEntity.Value?.As<CCSPlayerController>();
        return HookResult.Continue;
    }

    public static HookResult TakeDamageOld(DynamicHook hook)
    {
        var info = hook.GetParam<CTakeDamageInfo>(1);

        if (info.Attacker.Value?.DesignerName != "player" || info.Attacker.Value == null)
        {
            return HookResult.Continue;
        }

        var player = info.Attacker.Value.As<CCSPlayerController>();

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
            Server.PrintToChatAll($"{Prefix} {ChatColors.Red}{player?.PlayerName} {ChatColors.White}Adlı oyuncu öldüğü için sütlükten atılmıştır!!");
        }
        return HookResult.Continue;
    }
    
    public static HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        var sutPlayers = Instance.sutList;
        if (sutPlayers.Count > 0)
        {
            foreach (var player in sutPlayers)
            {
                player.sutBoz();
            }
        }
        
        return HookResult.Continue;
    }

    
    public static void PrecacheMonitfests(ResourceManifest manifest)
    {
        if (Instance.Config.SutModeli != null)
        {
            manifest.AddResource(Instance.Config.SutModeli);
        }
    }

    public static void OnTick()
    {
        if (Instance.sutList.Count == 0 || !Instance.Config.SutGokkusagi)
            return;

        foreach (var player in Instance.sutList)
        {
            if (!player.IsValid()) continue;
            
            if (!Instance.PreviousColors.ContainsKey(player))
            {
                Instance.PreviousColors[player] = Schema.GetSchemaValue<Color>(player.PlayerPawn.Value.Handle, "CBaseModelEntity", "m_clrRender");
            }
            
            Color newColor = GetNextRainbowColor(player);
            player.SetColour(newColor);
        }
    }
    
    public static HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        Server.PrintToChatAll($"{Prefix} {ChatColors.White}Süt olma süresi {Instance.Config.SutSuresi} saniye sonra sona erecektir.");
        Instance.AddTimer(Instance.Config.SutSuresi, () =>
        {
            Instance.SutAktif = false;
            Server.PrintToChatAll($"{Prefix} {ChatColors.White}Süt alımı kapatılmıştır.");
        });
        return HookResult.Continue;
    }
}