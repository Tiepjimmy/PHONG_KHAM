using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class SupplierRepository(IUnitOfWork unitOfWork) : Repository<Supplier>(unitOfWork), ISupplierRepository
    {
	}
}
