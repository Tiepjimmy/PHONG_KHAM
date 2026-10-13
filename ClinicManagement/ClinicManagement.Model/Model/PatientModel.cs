using ClinicManagement.Model.Entity;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Model.Model
{
    public class PatientModel : Patient
    {
        public List<PatientDetailViewModel>? ListPatientDetail { get; set; }
    }
}
