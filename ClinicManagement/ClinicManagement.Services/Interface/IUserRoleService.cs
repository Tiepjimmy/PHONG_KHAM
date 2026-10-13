using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;

namespace ClinicManagement.Services.Interface
{
    public interface IUserRoleService
    {
        public IResults<UserRole> GetAllData();
        public Task<IResult<UserRoleModel>> GetByUserGroupId(string listUserGroupId, bool isUseManage);
        public Task<IResult<bool>> DeleteByUserGroupId(Guid userGroupId);
        public Task<IResult<bool>> Insert(Guid userGroupId);
        public Task<IResult<bool>> Update(UserRoleRequest request);
    }
}
