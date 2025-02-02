/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */


using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Localization;
using static Events;

namespace Sutol;

public partial class Main : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Sutol";
    public override string ModuleAuthor => "Abby";
    public override string ModuleVersion => "1.0.1";
    public static Main Instance { get; set; }

    public static string Prefix = $" {ChatColors.Gold}[{ChatColors.Blue}ABBY{ChatColors.Gold}] {ChatColors.White}";
    public Config Config { get; set; }
    public List<CCSPlayerController>? sutList { get; set; }
    public bool SutAktif = false;
    public Dictionary<CCSPlayerController, Color> PreviousColors { get; set; }
    public Dictionary<CCSPlayerController, string>? playerOldModels { get; set; }

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnServerPrecacheResources>(PrecacheMonitfests);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(TakeDamageOld, HookMode.Pre);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook(OnWeaponCanUse, HookMode.Pre);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect, HookMode.Pre);
        RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Pre);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        RegisterListener<Listeners.OnTick>(OnTick);
        AddCommands();
    }

    public override void Unload(bool hotReload)
    {
        RemoveListener<Listeners.OnServerPrecacheResources>(PrecacheMonitfests);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(TakeDamageOld, HookMode.Pre);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Unhook(OnWeaponCanUse, HookMode.Pre);
        DeregisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect, HookMode.Pre);
        DeregisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Pre);
        DeregisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        RemoveListener<Listeners.OnTick>(OnTick);
        RemoveCommands();
    }
    
    private void AddCommands()
    {
        AddCommand("sutol", "süt olma komutu", sutOl);
        AddCommand("sutboz", "Sütlüğü bozma komutu", sutBoz);
        AddCommand("sut0", "Adminlerin sütleri silebilmesini sağlayan komut", sutSil);
    }

    private void RemoveCommands()
    {
        RemoveCommand("sutol", sutOl);
        RemoveCommand("sutboz", sutBoz);
        RemoveCommand("sut0", sutSil);
    }

    public void OnConfigParsed(Config config)
    {
        Config = config;
    }
}