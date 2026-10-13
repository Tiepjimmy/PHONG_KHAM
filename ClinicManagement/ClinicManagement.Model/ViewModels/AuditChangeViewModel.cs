namespace ClinicManagement.Model.ViewModels
{
    public class AuditChangeViewModel : BaseViewModel
    {
        public Guid? IdChange { get; set; }
        public string? Action { get; set; }
        public string? EntityName { get; set; }
        public string? Changes { get; set; }
    }
}
