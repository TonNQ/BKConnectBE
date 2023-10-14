using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.ClassManagement
{
    public class ClassDto
    {
        [JsonPropertyName("class_id")]
        public long ClassId { get; set; }

        [JsonPropertyName("class_name")]
        public string ClassName { get; set; }

        [JsonPropertyName("faculty_id")]
        public string FacultyId { get; set; }

        [JsonPropertyName("faculty_name")]
        public string FacultyName { get; set; }
    }
}