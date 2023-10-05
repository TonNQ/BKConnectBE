namespace BKConnect.BKConnectBE.Common
{
    public static class MsgNo
    {
        //Success
        public static readonly string SUCCESS_REGISTER = "Đăng ký tài khoản thành công";
        public static readonly string SUCCESS_LOGIN = "Đăng nhập thành công";
        public static readonly string SUCCESS_GET_PROFILE = "Lấy thông tin người dùng thành công";
        public static readonly string SUCCESS_GET_TOKEN = "Lấy access token thành công";

        //Error
        public static readonly string ERROR_REGISTER = "Đăng ký tài khoản thất bại";
        public static readonly string ERROR_EMAIL_HAS_USED = "Email này đã được sử dụng";
        public static readonly string  ERROR_EMAIL_NOT_REGISTERED = "Email này chưa được đăng ký";
        public static readonly string ERROR_PASSWORD_WRONG = "Mật khẩu chưa chính xác";
        public static readonly string ERROR_TOKEN_INVALID = "Vui lòng đăng nhập để tiếp tục";
        public static readonly string ERROR_ACCOUNT_NOT_ACTIVE = "Vui lòng kích hoạt tài khoản";
        public static readonly string ERROR_INPUT_INVALID = "Dữ liệu nhập vào không hợp lệ";

        //Error internal service
        public static readonly string ERROR_INTERNAL_SERVICE = "Một lỗi đã xảy ra trên hệ thống, thử lại sau";
    }
}