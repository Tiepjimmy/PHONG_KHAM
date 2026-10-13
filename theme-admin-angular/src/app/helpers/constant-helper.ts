// Same-origin API: nginx (Docker) or proxy.conf.json (ng serve) forwards /api to the .NET backend
export const URL_API = '/api/';
export const PROJECT_NAME = 'Quản lý phòng khám đa khoa Trung Cang';
export const PROJECT_SHORT_NAME = 'Trung Cang';
export const DOMAIN_ROUTER = 'quan-tri';
export const LOGO_HEADER_NAME = 'assets/img/logo/icon.jpg';
export const LOGO_LOGIN_NAME = 'assets/img/logo/icon.jpg';
export const STRING_EMPTY = '';
export const SOURCE = '/';
export const SPACE = ' ';
export const PROMOTE = '#';
export const QUESTION_MARK = '?';
export const OPEN_BRACKET = '[';
export const CLOSE_BRACKET = ']';
export const BASE_IMAGE = 'assets/img/base.jpg';
export const AUTHEN = 'authen';
export const DATE_FORMAT = 'dd/MM/yyyy';
export const DATE_FORMAT_DDMMyyyy1 = 'DD/MM/yyyy';
export const DATE_FORMAT_DDMMyyyy = 'DDMMyyyy';
export const DATE_FORMAT_yyyy_MM_DD = 'yyyy-MM-DD';
export const DATE_FORMAT_YYYYMMDD = 'YYYY/MM/DD';
export const DATE_FORMAT_ddMMyyyyhhmmss = 'dd/MM/yyyy hh:mm:ss';
export const DATE_FORMAT_ddMMyyyyHHmmss = 'dd/MM/yyyy HH:mm:ss';
export const NOTIFICATION_TITLE = 'Thông báo';
export const NOTIFICATION_TITLE_CONFIRM = 'Thông báo';
export const SUCCESS = 'success';
export const INFO = 'info';
export const WARNING = 'warning';
export const ERROR = 'error';
export const INTERNAL_SERVER_ERROR =
  'Đã xảy ra lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị viên';
export const INTERNAL_SERVER_SAVE = 'Đã xảy ra lỗi trong quá trình lưu dữ liệu';
export const LESS_THAN_EQUALS_ZERO = 'Vui lòng nhập số <= 0';
export const UN_AUTHORIZED =
  'Phiên làm việc của bạn đã hết hạn. Vui lòng đăng nhập lại';
export const TYPE_SAVE = 'Save';
export const DOT = '.';
export const COMMA = ',';
export const DOC = 'doc';
export const DOCX = 'docx';
export const XLS = 'xls';
export const XLSX = 'xlsx';
export const BMP = 'bmp';
export const GIF = 'gif';
export const JPEG = 'jpeg';
export const JPG = 'jpg';
export const PNG = 'png';
export const Z7Z = '7z';
export const RAR = 'rar';
export const TAR = 'tar';
export const ENC = 'enc';
export const ZIP = 'zip';
export const PDF = 'pdf';
export const PPT = 'ppt';
export const PPTX = 'pptx';
export const ADD_MODAL_TITLE = 'Thêm mới thông tin';
export const UPDATE_MODAL_TITLE = 'Cập nhật thông tin';
export const REPLICATION_MODAL_TITLE = 'Nhân bản thông tin';
export const VIEW_MODAL_TITLE = 'Xem thông tin';
export const PRINT_ALL_MODAL_TITLE = 'In danh sách ';
export const IMAGE_FORMAT_ERROR = 'Vui lòng chọn File có định dạng ảnh';
export const FILE_ATTACH_FORMAT_ERROR =
  'Vui lòng chọn File có định dạng PDF, DOC, JPG, JPEG, PNG';
export const MY_FILE = 'myFile';
export const MY_FILES = 'myFiles';
export const CONFIRM_DELETE = 'Bạn có chắc chắn muốn xóa?';
export const CHECK_TWO_DATE = 'Ngày bắt đầu phải nhỏ hơn Ngày kết thúc!';
export const FILE_REQUIRED = 'Vui lòng chọn File';
export const FILE_LIMITED = 'Hệ thống chỉ chấp nhập 10 File trong 1 lần upload';
export const FILE_WRONG_FORMAT =
  'Sai định dạng File. Vui lòng chọn File có định dạng doc, docx, xls, xlsx, bmp, gif, jpeg, jpg, png, 7z, rar, tar, enc, zip, pdf, ppt, pptx';
export const FILE_MAX_SIZE = 'Vui lòng chọn File có dung lượng tối đa 10mb';
export const IMPORT_TEMPLATE = '_Template';
export const FILE_EXCEL_WRONG_FORMAT =
  'Sai định dạng File. Vui lòng chọn File có định dạng xls, xlsx';
export const PATH_TEMPLATE_EXCEL = '/assets/template/';
export const PATTERN_NUMBER_CURRENCY = /\B(?=(\d{3})+(?!\d))/g;
export const PATTERN_NUMBER_REMOVE_CURRENCY = /\$\s?|(,*)/g;
export const ERROR_GUIDE =
  'Nhập kiểu dữ liệu dưới dạng xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx';
export const ERROR_GUIDE_PATTERN = new RegExp(
  '^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$',
  'i'
);
