using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class RegionRepository(IUnitOfWork unitOfWork) : Repository<Region>(unitOfWork), IRegionRepository
    {
    }
}
