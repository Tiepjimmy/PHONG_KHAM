using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IRegionService
    {
        public Task<IResults<RegionViewModel>> GetAll(RegionSearchRequest request);
        public Task<IResult<bool>> Save(RegionRequest request, string typeSave);
        public IResults<Region> GetAllData();
        public Task<IResult<Region>> GetById(Guid id);
    }
}
