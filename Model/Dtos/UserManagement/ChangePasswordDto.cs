using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common;
using BKConnectBE.Common.Attributes;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(Constants.REGEX_PASSWORD, ErrorMessage = "{0} không hợp lệ!")]
        [JsonPropertyName("current_password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(Constants.REGEX_PASSWORD, ErrorMessage = "{0} không hợp lệ!")]
        [NotEqualToPassword(nameof(CurrentPassword))]
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }
    }
}