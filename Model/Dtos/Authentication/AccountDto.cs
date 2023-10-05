using System.ComponentModel.DataAnnotations;

namespace BKConnectBE.Model.Dtos.Authentication
{
    public class AccountDto
    {
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^.+@.*dut\.udn\.vn$", ErrorMessage = "{0} không hợp lệ!")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "{0} không hợp lệ!")]
        public string Password { get; set; }
    }
}