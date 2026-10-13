namespace ClinicManagement.Model.Response
{
    public class ExportToExcelResponse
    {
        public int ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public byte[]? FileContent { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
