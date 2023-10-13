namespace BKConnect.BKConnectBE.Common
{
    public static class MsgNo
    {
        //Success
        public static readonly string SUCCESS_REGISTER = "Đăng ký tài khoản thành công";
        public static readonly string SUCCESS_ACTIVE_ACCOUNT = "Kích hoạt tài khoản thành công";
        public static readonly string SUCCESS_LOGIN = "Đăng nhập thành công";
        public static readonly string SUCCESS_GET_PROFILE = "Lấy thông tin người dùng thành công";
        public static readonly string SUCCESS_GET_TOKEN = "Lấy access token thành công";
        public static readonly string SUCCESS_GET_ROOMS_OF_USER = "Lấy danh sách phòng chat của người dùng thành công";
        public static readonly string SUCCESS_GET_USERS = "Tìm kiếm danh sách người dùng thành công";
        public static readonly string SUCCESS_EMAIL_FORGOT_PASSWORD = "Vui lòng kiểm tra email để đặt lại mật khẩu";
        public static readonly string SUCCESS_RESET_PASSWORD = "Đặt lại mật khẩu thành công";
        public static readonly string SUCCESS_TOKEN_VALID = "Đường dẫn hợp lệ";
        public static readonly string SUCCESS_UPDATE_PROFILE = "Cập nhật thành công";
        public static readonly string SUCCESS_LOGOUT = "Đăng xuất thành công";

        //Error
        public static readonly string ERROR_REGISTER = "Đăng ký tài khoản thất bại";
        public static readonly string ERROR_EMAIL_HAS_USED = "Email này đã được sử dụng";
        public static readonly string ERROR_USER_NOT_FOUND = "Người dùng không tồn tại";
        public static readonly string ERROR_USER_HAD_ACTIVED = "Tài khoản này đã được kích hoạt rồi";
        public static readonly string ERROR_SEND_EMAIL = "Gửi email thất bại. Vui lòng thử lại sau";
        public static readonly string ERROR_INPUT_INVALID = "Dữ liệu nhập vào không hợp lệ";
        public static readonly string ERROR_EMAIL_NOT_REGISTERED = "Email này chưa được đăng ký";
        public static readonly string ERROR_PASSWORD_WRONG = "Mật khẩu chưa chính xác";
        public static readonly string ERROR_PASSWORD_NOT_RESETED = "Mật khẩu chưa được đặt lại";
        public static readonly string ERROR_TOKEN_INVALID = "Vui lòng đăng nhập để tiếp tục";
        public static readonly string ERROR_ACCOUNT_NOT_ACTIVE = "Vui lòng kích hoạt tài khoản";
        public static readonly string ERROR_UNAUTHORIZED = "Bạn không có quyền truy cập";
        public static readonly string ERROR_CURRENT_PASSWORD_WRONG = "Mật khẩu hiện tại chưa chính xác";
        public static readonly string ERROR_LOGOUT = "Đăng xuất thất bại";

        //Error internal service
        public static readonly string ERROR_INTERNAL_SERVICE = "Một lỗi đã xảy ra trên hệ thống, thử lại sau";
    }
}