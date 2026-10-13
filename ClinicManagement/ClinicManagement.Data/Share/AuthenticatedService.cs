using ClinicManagement.Data.Interface;
using ClinicManagement.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClinicManagement.Data.Share
{
    public class AuthenticatedService(IHttpContextAccessor httpContextAccessor, IDapperRepository dapperRepository) : IAuthenticatedService
    {
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid Id
        {
            get
            {
                var firstUser = _dapperRepository.QueryFirstOrDefault<Guid>("SELECT TOP 1 Id FROM [User] WHERE IsActive = 1 AND IsDeleted = 0");
                var result = _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
                return !string.IsNullOrEmpty(result?.ToString()) ? Guid.Parse(result) : firstUser;
            }
        }

        public UserInfo UserInfo => _dapperRepository.QueryFirstOrDefault<UserInfo>(
              "     SELECT TOP 1 u.Id"
            + "           ,u.UserName"
            + "           ,u.PersonId"
            + "     FROM [User] AS u"
            + $"    WHERE u.Id = '{Id}' AND u.IsActive = 1 AND u.IsDeleted = 0"
            ) ?? new();
    }
}
