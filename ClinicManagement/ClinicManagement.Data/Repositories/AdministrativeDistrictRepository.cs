using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class AdministrativeDistrictRepository(IUnitOfWork unitOfWork) : Repository<AdministrativeDistrict>(unitOfWork), IAdministrativeDistrictRepository
    {
	}
}
