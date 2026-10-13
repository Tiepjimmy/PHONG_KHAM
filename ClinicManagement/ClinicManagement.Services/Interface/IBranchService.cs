using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IBranchService
    {
        public Task<IResults<BranchViewModel>> GetAll(BranchSearchRequest request);
        public Task<IResult<bool>> Save(BranchRequest request, string typeSave);
        public IResults<Branch> GetAllData();
        public Task<IResult<Branch>> GetById(Guid id);
        public IResults<Branch> GetAllDataByRegionId(Guid? regionId);
        public IResults<Branch> GetAllDataByAreaId(Guid? areaId);
    }
}
