using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IObjectMenuService
    {
        public IResults<ObjectMenu> GetAllData();
        public Task<IResults<ObjectMenuViewModel>> GetAll(ObjectMenuSearchRequest request);
        public Task<IResult<bool>> Save(ObjectMenuRequest request, string typeSave);
        public Task<IResult<ObjectMenu>> GetById(Guid id);
        public Task<IResult<bool>> SaveIsActive(Guid id);
    }
}
