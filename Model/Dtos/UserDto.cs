using BKConnectBE.Model.Entities;
using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos;

public class UserDto
{
    public UserDto() { }

    public UserDto(User user)
    {
        Name = user.Name;
        Id = user.Id;
        BirthDay = user.DateOfBirth;
        Gender= user.Gender;
    }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("gender")]
    public bool Gender { get; set; }

    [JsonPropertyName("birthday")]
    public DateTime BirthDay { get; set; }
}
