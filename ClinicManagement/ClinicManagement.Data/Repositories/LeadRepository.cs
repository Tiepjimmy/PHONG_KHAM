using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class LeadRepository(IUnitOfWork unitOfWork) : Repository<Leads>(unitOfWork), ILeadRepository
    {
    }
}
