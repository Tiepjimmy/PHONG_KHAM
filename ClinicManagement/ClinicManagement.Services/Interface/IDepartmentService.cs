using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IDepartmentService
    {
        public Task<IResults<DepartmentViewModel>> GetAll(DepartmentSearchRequest request);
        public Task<IResult<bool>> Save(DepartmentRequest request, string typeSave);
        public IResults<Department> GetAllData();
        public Task<IResult<Department>> GetById(Guid id);
        public IResults<Department> GetAllDataByBranchId(Guid? branchId);
    }
}
