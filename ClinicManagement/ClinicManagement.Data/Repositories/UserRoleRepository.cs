using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class UserRoleRepository(IUnitOfWork unitOfWork) : Repository<UserRole>(unitOfWork), IUserRoleRepository
    {
    }
}
