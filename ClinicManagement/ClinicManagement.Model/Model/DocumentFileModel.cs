namespace ClinicManagement.Model.Model
{
    public class DocumentFileModel : BaseModel
    {
        public Guid? TestDemoId { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? FileType { get; set; }
        public string? FileNameHas { get; set; }
        public string? FileNameUrl { get; set; }
    }
}
