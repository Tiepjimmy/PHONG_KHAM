namespace ClinicManagement.Model.ViewModels.Custom
{
    public class AuditChangeCustomViewModel : AuditChangeViewModel
    {
        public DateTime CreatedDate { get; set; }
        public string? UserName { get; set; }
    }
}
