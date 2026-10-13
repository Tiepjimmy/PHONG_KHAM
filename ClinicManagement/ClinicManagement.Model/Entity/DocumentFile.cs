namespace ClinicManagement.Model.Entity
{
    public class DocumentFile : BaseEntity
    {
        public Guid? TestDemoId { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? FileType { get; set; }
        public string? FileNameHas { get; set; }
    }
}
