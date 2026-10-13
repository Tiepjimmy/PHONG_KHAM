namespace ClinicManagement.Model.Request
{
    public class AuditChangeRequest : BaseRequest
    {
        public string? Action { get; set; }
        public string? PropertyName { get; set; }
        public string? ValueOld { get; set; }
        public string? ValueNew { get; set; }
    }
}
