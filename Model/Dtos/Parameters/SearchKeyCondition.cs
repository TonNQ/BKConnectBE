using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.Parameters
{
    public class SearchKeyCondition
    {
        [JsonPropertyName("search_key")]
        public string SearchKey { get; set; }

        public SearchKeyCondition()
        {
            SearchKey = "";
        }
    }
}