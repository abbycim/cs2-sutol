using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace Sutol;

public partial class Main
{
    
    private enum SutCondition
    {
        Valid,
        InvalidPlayer,
        WrongTeam,
        MaxSutReached,
        AlreadySut,
        SutBozDisabled,
        NotSut,
        NoSutPlayers
    }

    private SutCondition CheckSutConditions(CCSPlayerController? player, CommandType commandType)
    {
        if (player == null || !player.IsValid())
            return SutCondition.InvalidPlayer;

        return commandType switch
        {
            CommandType.SutOl => CheckSutOlConditions(player),
            CommandType.SutBoz => CheckSutBozConditions(),
            CommandType.SutSil => CheckSutSilConditions(),
            _ => SutCondition.Valid
        };
    }

    private SutCondition CheckSutOlConditions(CCSPlayerController player)
    {
        if (player.TeamNum != 2)
            return SutCondition.WrongTeam;

        if (sutList.Count >= Config.MaxSut)
            return SutCondition.MaxSutReached;

        if (player.sutOl())
            return SutCondition.AlreadySut;

        return SutCondition.Valid;
    }

    private SutCondition CheckSutBozConditions()
    {
        if (!Config.SutBozEnabled)
            return SutCondition.SutBozDisabled;

        return SutCondition.Valid;
    }

    private SutCondition CheckSutSilConditions()
    {
        if (!sutList.Any())
            return SutCondition.NoSutPlayers;

        return SutCondition.Valid;
    }

    private void HandleConditionError(CommandInfo commandInfo, SutCondition condition)
    {
        var message = condition switch
        {
            SutCondition.InvalidPlayer => "Süt olabilmek için yaşıyor olmanız gerekmektedir.",
            SutCondition.WrongTeam => $"Bu takımı kullanabilmek için {ChatColors.Red}süt olamazsınız.",
            SutCondition.MaxSutReached => $"Eklenti ayarlarında belirtilen süt sayısına ulaşıldığından {ChatColors.Red}süt olamazsınız.",
            SutCondition.AlreadySut => $"{ChatColors.Red}Zaten sütsünüz!!",
            SutCondition.SutBozDisabled => $"{ChatColors.Red}Süt bozma aktif değil!!",
            SutCondition.NotSut => $"{ChatColors.Red}Süt Değilsiniz!!",
            SutCondition.NoSutPlayers => $"{ChatColors.Red}Herhangi bir süt bulunmamakta!!",
            _ => "Bilinmeyen hata oluştu"
        };

        commandInfo.ReplyToCommand($"{Prefix} {ChatColors.White}{message}");
    }
}

public enum CommandType
{
    SutOl,
    SutBoz,
    SutSil
}