using ClinicManagement.Data.Interface;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels.Custom;

namespace ClinicManagement.Services.Interface
{
    public interface ILocationTrackingService
    {
        public Task<IResults<LocationTrackingCustomViewModel>> GetAll(LocationTrackingSearchRequest request);
        public IResult<bool> DeleteData();
    }
}
