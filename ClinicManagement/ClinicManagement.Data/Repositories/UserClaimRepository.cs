using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class UserClaimRepository(IUnitOfWork unitOfWork) : Repository<UserClaim>(unitOfWork), IUserClaimRepository
    {
	}
}
