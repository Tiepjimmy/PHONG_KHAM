using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class LocationTrackingRepository(IUnitOfWork unitOfWork) : Repository<LocationTracking>(unitOfWork), ILocationTrackingRepository
    {
    }
}
