using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.Parameters
{
    public class LongKeyCondition
    {
        [JsonPropertyName("search_key")]
        public long SearchKey { get; set; }
    }
}