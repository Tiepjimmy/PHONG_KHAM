using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class UserRepository(IUnitOfWork unitOfWork) : Repository<User>(unitOfWork), IUserRepository
    {
    }
}
