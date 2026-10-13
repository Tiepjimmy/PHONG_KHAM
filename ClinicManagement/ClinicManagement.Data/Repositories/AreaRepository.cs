using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class AreaRepository(IUnitOfWork unitOfWork) : Repository<Area>(unitOfWork), IAreaRepository
    {
    }
}
