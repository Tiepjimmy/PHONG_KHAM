using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class AdministrativeUnitRepository(IUnitOfWork unitOfWork) : Repository<AdministrativeUnit>(unitOfWork), IAdministrativeUnitRepository
    {
	}
}
