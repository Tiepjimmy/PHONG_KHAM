namespace ClinicManagement.Model.Helper
{
    public class ConstantHelper
    {
        public static readonly string ConnectionStrings = "ConnectionStrings";
        public static readonly string MainConnectionString = "MainConnectionString";
        public static readonly string Import = "Import";
        public static readonly string UploadFile = "UploadFile";
        public static readonly string Underlined = "_";
        public static readonly string Minus = "-";
        public static readonly string Space = " ";
        public static readonly string Source = "/";
        public static readonly string Colon = ":";
        public static readonly string Comma = ",";
        public static readonly string CommaSpace = ", ";
        public static readonly string Dot = ".";
        public static readonly string SemiColon = ";";
        public static readonly string OpeningParenthesis = "(";
        public static readonly string CloseParenthesis = ")";
        public static readonly string OpenBracket = "[";
        public static readonly string CloseBracket = "]";
        public static readonly string SourceN = "\n";
        public static readonly string Break = "<br />";
        public static readonly string Page = "@Page";
        public static readonly string PageSize = "@PageSize";
        public static readonly string SortField = "@SortField";
        public static readonly string SortOrder = "@SortOrder";
        public static readonly string FormatDate_ddMMyyyy = "dd/MM/yyyy";
        public static readonly string FormatDate_ddMMyyyyhmmss = "ddMMyyyyhmmss";
        public static readonly string FormatDate_ddMMyyyyhmmss1 = "dd/MM/yyyy h:mm:ss";
        public static readonly string DownloadExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static readonly string xlsx = ".xlsx";
        public static readonly string xls = ".xls";
        public static readonly string docx = ".docx";
        public static readonly string Zero = "0";
        public static readonly string One = "1";
        public static readonly string NotFound = "Không tìm thấy";

        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol", Id = "id";
        }

        public static class JwtClaims
        {
            public const string ApiAccess = "api_access";
        }

        public static class Message
        {
            public static readonly string GetAllSuccessfully = "Lấy thông tin dữ liệu thành công";
            public static readonly string Successfully = "Successfully";
            public static readonly string InternalServer = "Internal Server Error";
            public static readonly string ModelStateMessage = "Model State Invalid";
            public static readonly string ImportSuccess = "Import dữ liệu thành công";
            public static readonly string SaveSuccess = "Lưu thông tin thành công";
            public static readonly string CannotImport = "Không thể tải dữ liệu lên, vui lòng liên hệ với quản trị viên";
            public static readonly string ImportFileNull = "Không thể tải dữ liệu lên, do trong file không có nội dung";
            public static readonly string FileCorrectFormat = "File tải lên chưa đúng định dạng, hệ thống chỉ tiếp nhận file có đuôi là xls, xlsx";
            public static readonly string FileRequired = "Yêu cầu chọn file";
            public static readonly string UserInActiveMessage = "Tài khoản hoặc mật khẩu không đúng";
            public static readonly string AccountInActive = "Tài khoản của bạn đang bị khóa, vui lòng liên hệ với quản trị viên";
            public static readonly string AccountDeleted = "Tài khoản của bạn đã bị xóa, vui lòng liên hệ với quản trị viên";
            public static readonly string ReportSuccess = "Gửi báo cáo thành công";
            public static readonly string RecordNotFoundMessage = "Không tìm thấy bản ghi";
            public static readonly string NameExist = "Tên {0} đã tồn tại, vui lòng nhập tên khác";
            public static readonly string MetaLinkExist = "Đường dẫn {0} đã tồn tại, vui lòng nhập đường dẫn khác";
            public static readonly string CodeExist = "Mã {0} đã tồn tại, vui lòng nhập mã khác";
            public static readonly string DeleteExistFail = "Không thể xóa dữ liệu danh mục này do dữ liệu đang được sử dụng";
            public static readonly string DeleteSuccess = "Xóa thông tin thành công";
            public static readonly string UploadFileSuccess = "Upload file thành công";
            public static readonly string UploadFileFail = "Đã xảy ra lỗi trong quá trình upload file";
            public static readonly string UserNameExist = "UserName {0} đã tồn tại, vui lòng nhập username khác";
            public static readonly string ResetPassword = "Đặt lại mật khẩu thành công";
            public static readonly string NotExist = " {0} không tồn tại trong cơ sở dữ liệu";
        }

        public class TypeSave
        {
            public const string Insert = "Insert";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string UpdateProfile = "UpdateProfile";
            public const string ResetPassword = "ResetPassword";
            public const string ChangePassword = "ChangePassword";
            public const string UpdateResult = "UpdateResult";
        }

        public class FolderName
        {
            public const string PERSON_IMAGE = "PERSON_IMAGE";
            public const string TEST_DEMO_UPLOAD = "TEST_DEMO_UPLOAD";
        }

        public class ExcelStyle
        {
            public static readonly string BackgroundColor = "#16548d";
            public static readonly string FontColor = "#fff";
            public static readonly string FontName = "Calibri";
        }
    }
}
