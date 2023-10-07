using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common;

namespace BKConnectBE.Model.Dtos.Authentication
{
    public class EmailDto
    {
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(Constants.REGEX_EMAIL, ErrorMessage = "{0} không hợp lệ!")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}