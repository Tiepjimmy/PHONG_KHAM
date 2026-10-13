using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IAreaService
    {
        public Task<IResults<AreaViewModel>> GetAll(AreaSearchRequest request);
        public Task<IResult<bool>> Save(AreaRequest request, string typeSave);
        public IResults<Area> GetAllData();
        public Task<IResult<Area>> GetById(Guid id);
        public IResults<Area> GetAllDataByRegionId(Guid? regionId);
    }
}
