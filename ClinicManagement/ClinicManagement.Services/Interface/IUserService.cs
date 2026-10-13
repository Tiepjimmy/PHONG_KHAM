using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IUserService
    {
        public Task<IResults<UserViewModel>> GetAll(UserSearchRequest request);
        public Task<IResult<bool>> Save(UserRequest request, string typeSave);
        public IResults<User> GetAllData();
        public Task<IResult<UserModel>> GetById(Guid id);
        public Task<IResult<bool>> ResetPassword(Guid id);
        public Task<IResult<bool>> SaveIsActive(Guid id);
        public Task<IResult<bool>> SaveIsAdmin(Guid id);
        public Task<IResult<bool>> ChangePassword(ChangePasswordRequest request);
        public Task<bool> InsertUserAfter(Guid personId, string? fullName, string typeSave);
    }
}
