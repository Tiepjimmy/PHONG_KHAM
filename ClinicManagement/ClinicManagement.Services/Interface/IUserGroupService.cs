using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IUserGroupService
    {
        public IResults<UserGroup> GetAllData();
        public Task<IResults<UserGroupViewModel>> GetAll(UserGroupSearchRequest request);
        public Task<IResult<bool>> Save(UserGroupRequest request, string typeSave);
        public Task<IResult<UserGroup>> GetById(Guid id);
    }
}
