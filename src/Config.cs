using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

public class Config : BasePluginConfig
{
    [JsonPropertyName("max_sut")] public int MaxSut { get; set; } = 3;

    [JsonPropertyName("SutModeli")] public string SutModeli { get; set; } = "";
    [JsonPropertyName("SutBozEnabled")] public bool SutBozEnabled { get; set; } = true;
    [JsonPropertyName("SutSuresi")] public float SutSuresi { get; set; } = 30f;
    [JsonPropertyName("GokkusagiSut")] public bool SutGokkusagi { get; set; } = true;
}