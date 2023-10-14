using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.Parameters
{
    public class SearchKeyConditionWithPage
    {
        [JsonPropertyName("search_key")]
        public string SearchKey { get; set; }

        [JsonPropertyName("page_index")]
        public int PageIndex { get; set; }

        public SearchKeyConditionWithPage()
        {
            PageIndex = 1;
        }
    }
}