using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class BranchRepository(IUnitOfWork unitOfWork) : Repository<Branch>(unitOfWork), IBranchRepository
    {
    }
}
