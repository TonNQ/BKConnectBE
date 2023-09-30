using System.Text.Json.Serialization;

namespace BKConnect.BKConnectBE.Common;

public class Responses
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; }
}