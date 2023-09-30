using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BKConnect.Model.Dtos.User;

public class UpdateUserDto
{
    [JsonPropertyName("userId")]
    [Required]
    public string UserId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
