using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class AdministrativeProvinceRepository(IUnitOfWork unitOfWork) : Repository<AdministrativeProvince>(unitOfWork), IAdministrativeProvinceRepository
    {
	}
}
