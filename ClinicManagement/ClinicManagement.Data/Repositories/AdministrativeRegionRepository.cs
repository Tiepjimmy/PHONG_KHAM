using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class AdministrativeRegionRepository(IUnitOfWork unitOfWork) : Repository<AdministrativeRegion>(unitOfWork), IAdministrativeRegionRepository
    {
	}
}
