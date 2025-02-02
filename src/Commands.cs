using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace Sutol;

public partial class Main
{
    public void sutOl(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var condition = CheckSutConditions(player, CommandType.SutOl);
        if (condition != SutCondition.Valid)
        {
            HandleConditionError(commandInfo, condition);
            return;
        }

        if (player!.sutOl())
        {
            Server.PrintToChatAll($"{Prefix} {ChatColors.Red}{player.PlayerName} {ChatColors.White}Adlı oyuncu süt oldu!!");
            player.RemoveWeapons();
        }
    }
    
    public void sutBoz(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var condition = CheckSutConditions(player, CommandType.SutBoz);
        if (condition != SutCondition.Valid)
        {
            HandleConditionError(commandInfo, condition);
            return;
        }

        if (player!.sutBoz())
        {
            Server.PrintToChatAll($"{Prefix} {ChatColors.Red}{player.PlayerName} {ChatColors.White}Adlı oyuncu sütlüğünü bozdu!!");
        }
        else
        {
            HandleConditionError(commandInfo, SutCondition.NotSut);
        }
    }

    [RequiresPermissions("@css/slay")]
    public void sutSil(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var condition = CheckSutConditions(player, CommandType.SutSil);
        if (condition != SutCondition.Valid)
        {
            HandleConditionError(commandInfo, condition);
            return;
        }

        var sutPlayers = sutList.ToList();
        foreach (var sutPlayer in sutPlayers)
        {
            sutPlayer.CommitSuicide(false, true);
            sutList.Remove(sutPlayer);
        }

        Server.PrintToChatAll(
            $"{Prefix} {ChatColors.Red}{player?.PlayerName} {ChatColors.White}Adlı admin tarafından bütün sütler silindi!!");
    }
}