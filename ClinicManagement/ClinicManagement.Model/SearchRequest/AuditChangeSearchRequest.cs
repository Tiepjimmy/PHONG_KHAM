namespace ClinicManagement.Model.SearchRequest
{
    public class AuditChangeSearchRequest : BaseSearchRequest
    {
        public Guid? IdChange { get; set; }
        public string? Action { get; set; }
        public string? EntityName { get; set; }
    }
}
