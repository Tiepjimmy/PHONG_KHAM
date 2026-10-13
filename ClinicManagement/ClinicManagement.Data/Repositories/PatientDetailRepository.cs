using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class PatientDetailRepository(IUnitOfWork unitOfWork) : Repository<PatientDetail>(unitOfWork), IPatientDetailRepository
    {
	}
}
