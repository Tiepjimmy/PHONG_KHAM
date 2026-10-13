namespace ClinicManagement.Model.Entity
{
    public class AuditChange : BaseEntity
    {
        public Guid? IdChange { get; set; }
        public string? Action { get; set; }
        public string? EntityName { get; set; }
        public string? Changes { get; set; }
    }
}
