using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class PatientRepository(IUnitOfWork unitOfWork) : Repository<Patient>(unitOfWork), IPatientRepository
    {
	}
}
