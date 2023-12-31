namespace BKConnectBE.Common
{
    public static class Constants
    {
        public const string EMAIL_ACTIVE_ACCOUNT_TITLE = "[BKConnect] Xác nhận tài khoản";
        public const string EMAIL_RESET_PASSWORD_TITLE = "[BKConnect] Đặt lại mật khẩu";
        public const string BECOME_FRIEND_MESSAGE = "Giờ đây hai bạn đã trở thành bạn bè. Hãy bắt đầu cuộc trò chuyện của mình nào!";
        public const string START_VIDEO_CALL = "Cuộc gọi video đã bắt đầu";
        public const string END_VIDEO_CALL = "Cuộc gọi đã kết thúc";
        public const string REGEX_EMAIL = @"^.+@.*dut\.udn\.vn$";
        public const string REGEX_PASSWORD = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
        public const string DEFAULT_AVATAR = "0.jpg";
        public const int DEFAULT_PAGE_SIZE = 10;
        public const int ATTEMPTS_IN_HANGFIRE = 3;
        public const int DELAY_IN_HANGFIRE = 30;
    }
}