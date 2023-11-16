namespace BKConnect.BKConnectBE.Common
{
    public static class MsgNo
    {
        //Success
        public static readonly string SUCCESS_REGISTER = "Đăng ký tài khoản thành công";
        public static readonly string SUCCESS_ACTIVE_ACCOUNT = "Kích hoạt tài khoản thành công";
        public static readonly string SUCCESS_LOGIN = "Đăng nhập thành công";
        public static readonly string SUCCESS_GET_PROFILE = "Lấy thông tin người dùng thành công";
        public static readonly string SUCCESS_GET_CLASS = "Lấy thông tin lớp học thành công";
        public static readonly string SUCCESS_GET_FACULTY = "Lấy thông tin khoa thành công";
        public static readonly string SUCCESS_GET_TOKEN = "Lấy access token thành công";
        public static readonly string SUCCESS_GET_ROOMS_OF_USER = "Lấy danh sách phòng chat của người dùng thành công";
        public static readonly string SUCCESS_GET_FRIEND_REQUESTS = "Lấy danh sách lời mời kết bạn thành công";
        public static readonly string SUCCESS_GET_USERS = "Tìm kiếm danh sách người dùng thành công";
        public static readonly string SUCCESS_GET_FRIENDS_OF_USER = "Tìm kiếm danh sách bạn bè của người dùng thành công";
        public static readonly string SUCCESS_GET_LIST_MESSAGES = "Lấy danh sách tin nhắn thành công";
        public static readonly string SUCCESS_GET_INFORMATION_OF_ROOM = "Lấy thông tin của phòng thành công";
        public static readonly string SUCCESS_GET_LIST_OF_MEMBERS_IN_ROOM = "Lấy danh sách thành viên trong phòng thành công";
        public static readonly string SUCCESS_GET_LIST_OF_NOTIFICATIONS = "Lấy danh sách thông báo thành công";
        public static readonly string SUCCESS_EMAIL_FORGOT_PASSWORD = "Vui lòng kiểm tra email để đặt lại mật khẩu";
        public static readonly string SUCCESS_RESET_PASSWORD = "Đặt lại mật khẩu thành công";
        public static readonly string SUCCESS_TOKEN_VALID = "Đường dẫn hợp lệ";
        public static readonly string SUCCESS_UPDATE_PROFILE = "Cập nhật thành công";
        public static readonly string SUCCESS_LOGOUT = "Đăng xuất thành công";
        public static readonly string SUCCESS_REMOVE_FRIEND_REQUEST = "Xóa lời mời kết bạn thành công";
        public static readonly string SUCCESS_CHECK_FRIEND_REQUEST = "Kiểm tra việc lời mời kết bạn thành công";
        public static readonly string SUCCESS_RESPONSE_FRIEND_REQUEST = "Trả lời lời mời kết bạn thành công";
        public static readonly string SUCCESS_UPDATE_FRIEND_REQUEST = "Cập nhật lời mời kết bạn thành công";
        public static readonly string SUCCESS_UPDATE_NOTIFICATION_STATUS = "Cập nhật trạng thái thông báo thành công";
        public static readonly string SUCCESS_UNFRIEND = "Hủy kết bạn thành công";
        public static readonly string SUCCESS_ADD_USER_TO_ROOM = "Thêm người dùng vào nhóm thành công";
        public static readonly string SUCCESS_REMOVE_USER_FROM_ROOM = "Xoá người dùng ra khỏi nhóm thành công";


        //Error
        public static readonly string ERROR_REGISTER = "Đăng ký tài khoản thất bại";
        public static readonly string ERROR_EMAIL_HAS_USED = "Email này đã được sử dụng";
        public static readonly string ERROR_USER_NOT_FOUND = "Người dùng không tồn tại";
        public static readonly string ERROR_CLASS_NOT_FOUND = "Lớp học không tồn tại";
        public static readonly string ERROR_FACULTY_NOT_FOUND = "Khoa không tồn tại";
        public static readonly string ERROR_ROOM_NOT_FOUND = "Phòng không tồn tại";
        public static readonly string ERROR_NOTIFICATION_NOT_FOUND = "Thông báo không tồn tại";
        public static readonly string ERROR_USER_HAD_ACTIVED = "Tài khoản này đã được kích hoạt rồi";
        public static readonly string ERROR_SEND_EMAIL = "Gửi email thất bại. Vui lòng thử lại sau";
        public static readonly string ERROR_INPUT_INVALID = "Dữ liệu nhập vào không hợp lệ";
        public static readonly string ERROR_EMAIL_NOT_REGISTERED = "Email này chưa được đăng ký";
        public static readonly string ERROR_PASSWORD_WRONG = "Mật khẩu chưa chính xác";
        public static readonly string ERROR_PASSWORD_NOT_RESETED = "Mật khẩu chưa được đặt lại";
        public static readonly string ERROR_TOKEN_INVALID = "Vui lòng đăng nhập để tiếp tục";
        public static readonly string ERROR_ACCOUNT_NOT_ACTIVE = "Vui lòng kích hoạt tài khoản";
        public static readonly string ERROR_CURRENT_PASSWORD_WRONG = "Mật khẩu hiện tại chưa chính xác";
        public static readonly string ERROR_LOGOUT = "Đăng xuất thất bại";
        public static readonly string ERROR_UNAUTHORIZED = "Bạn không có quyền truy cập";
        public static readonly string ERROR_UNHADLED_ACTION = "Bạn không thể thực hiện hành động này";
        public static readonly string ERROR_CREATE_FRIEND_REQUEST = "Gửi lời mời kết bạn thất bại";
        public static readonly string ERROR_RESPONSE_FRIEND_REQUEST = "Trả lời lời mời kết bạn thất bại";
        public static readonly string ERROR_REMOVE_FRIEND_REQUEST = "Xóa lời mời kết bạn thất bại";
        public static readonly string ERROR_UPDATE_FRIEND_REQUEST = "Cập nhật lời mời kết bạn thất bại";
        public static readonly string ERROR_RELATIONSHIP_NOT_FOUND = "Các bạn hiện không phải là bạn bè";
        public static readonly string ERROR_USER_NOT_IN_ROOM = "Bạn không có trong nhóm này";
        public static readonly string ERROR_ADD_USER_TO_ROOM = "Bạn không thể thêm người dùng vào nhóm";
        public static readonly string ERROR_USER_ALREADY_IN_ROOM = "Người dùng đã ở trong nhóm này rồi";


        //Error internal service
        public static readonly string ERROR_INTERNAL_SERVICE = "Một lỗi đã xảy ra trên hệ thống, thử lại sau";
    }
}