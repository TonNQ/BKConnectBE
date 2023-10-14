using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FacultyManagement
{
    public class FacultyDto
    {
        [JsonPropertyName("faculty_id")]
        public string FacultyId { get; set; }

        [JsonPropertyName("faculty_name")]
        public string FacultyName { get; set; }
    }
}