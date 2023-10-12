using System.ComponentModel.DataAnnotations;

namespace BKConnectBE.Model.Dtos.Authentications
{
    public class AccountDto
    {
        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^\d{9}@.+\.dut\.udn\.vn$", ErrorMessage = "{0} không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "{0} không hợp lệ!")]
        public string Password { get; set; }
    }
}