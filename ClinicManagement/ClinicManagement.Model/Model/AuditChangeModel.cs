using ClinicManagement.Model.Entity;

namespace ClinicManagement.Model.Model
{
    public class AuditChangeModel : AuditChange
    {
        public bool IsChanged { get; set; }
    }
}
