namespace BKConnectBE.Common
{
    public static class Constants
    {
        public const string EMAIL_ACTIVE_ACCOUNT_TITLE = "[BKConnect] Xác nhận tài khoản";
        public const string EMAIL_RESET_PASSWORD_TITLE = "[BKConnect] Đặt lại mật khẩu";
        public const string REGEX_EMAIL = @"^.+@.*dut\.udn\.vn$";
        public const string REGEX_PASSWORD = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

        public const int DEFAULT_PAGE_SIZE = 2;
    }
}