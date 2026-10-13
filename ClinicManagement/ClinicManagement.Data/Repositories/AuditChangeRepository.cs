using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class AuditChangeRepository(IUnitOfWork unitOfWork) : Repository<AuditChange>(unitOfWork), IAuditChangeRepository
    {
    }
}
