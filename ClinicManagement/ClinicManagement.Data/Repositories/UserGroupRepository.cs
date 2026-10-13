using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class UserGroupRepository(IUnitOfWork unitOfWork) : Repository<UserGroup>(unitOfWork), IUserGroupRepository
    {
    }
}
