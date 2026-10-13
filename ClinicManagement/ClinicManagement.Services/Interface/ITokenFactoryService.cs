using Newtonsoft.Json;
using ClinicManagement.Model.Option;
using ClinicManagement.Model.ViewModels;
using System.Security.Claims;

namespace ClinicManagement.Services.Interface
{
    public interface ITokenFactoryService
    {
        public Task<string> GenerateJwt(ClaimsIdentity identity
            , IJwtFactoryService jwtFactoryService
            , JwtIssuerOptions jwtOptions
            , JsonSerializerSettings serializerSettings
            , Guid id
            , string userName
            , string fullName
            , string avatar
            , string userGroupName
            , bool isAdmin
            , List<UserRoleViewModel> listMenu
            , List<Claim>? claims);
    }
}
