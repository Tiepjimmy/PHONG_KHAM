using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IUserClaimService
    {
        public Task<IResults<UserClaimViewModel>> GetAll(UserClaimSearchRequest request);
        public Task<IResult<bool>> Save(UserClaimRequest request, string typeSave);
        public IResults<UserClaim> GetAllData();
        public Task<IResult<UserClaim>> GetById(Guid id);
        public Task<IResult<bool>> SaveAfterUser(Guid userId, string? listUserGroupId);
    }
}
