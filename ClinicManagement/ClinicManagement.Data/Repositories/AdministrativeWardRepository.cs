using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class AdministrativeWardRepository(IUnitOfWork unitOfWork) : Repository<AdministrativeWard>(unitOfWork), IAdministrativeWardRepository
    {
	}
}
