using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class ObjectMenuRepository(IUnitOfWork unitOfWork) : Repository<ObjectMenu>(unitOfWork), IObjectMenuRepository
    {
    }
}
