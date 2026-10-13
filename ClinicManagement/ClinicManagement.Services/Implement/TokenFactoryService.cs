using Newtonsoft.Json;
using ClinicManagement.Model.Option;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using System.Security.Claims;

namespace ClinicManagement.Services.Implement
{
    public class TokenFactoryService : ITokenFactoryService
    {
        public async Task<string> GenerateJwt(ClaimsIdentity identity
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
            , List<Claim>? claims)
        {
            AuthenticateResponse response = new()
            {
                UserId = identity.Claims.Single(c => c.Type == "id").Value,
                Token = await jwtFactoryService.GenerateEncodedToken(userName, identity, claims ?? []),
                ExpireTime = (int)jwtOptions.ValidFor.TotalSeconds,
                Id = id,
                UserName = userName,
                FullName = fullName,
                Avatar = avatar,
                UserGroupName = userGroupName,
                IsAdmin = isAdmin,
                ListMenu = listMenu
            };
            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
