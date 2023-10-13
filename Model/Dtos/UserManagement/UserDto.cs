using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class UserDto
    {
        [JsonPropertyName("user_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gender")]
        public bool Gender { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("class_id")]
        public long ClassId { get; set; }

        [JsonPropertyName("class_name")]
        public string ClassName { get; set; }

        [JsonPropertyName("faculty_id")]
        public string FacultyId { get; set; }

        [JsonPropertyName("faculty_name")]
        public string FacultyName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}
