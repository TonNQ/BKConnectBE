using System.ComponentModel.DataAnnotations;
using BKConnectBE.Common.Attributes;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "{0} không hợp lệ!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "{0} không hợp lệ!")]
        [NotEqualToPassword(nameof(CurrentPassword))]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "{0} không hợp lệ!")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng!")]
        public string ConfirmPassword { get; set; }
    }
}