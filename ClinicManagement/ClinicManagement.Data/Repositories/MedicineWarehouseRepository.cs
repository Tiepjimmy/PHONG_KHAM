using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class MedicineWarehouseRepository(IUnitOfWork unitOfWork) : Repository<MedicineWarehouse>(unitOfWork), IMedicineWarehouseRepository
    {
	}
}
