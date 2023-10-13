using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Attributes;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class UserInputDto
    {
        [JsonPropertyName("user_id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [JsonPropertyName("gender")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [DateRange("1900-01-01")]
        [JsonPropertyName("birthday")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [JsonPropertyName("class_id")]
        public long ClassId { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}