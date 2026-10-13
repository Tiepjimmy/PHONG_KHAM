using System.Security.Claims;

namespace ClinicManagement.Services.Interface
{
    public interface IJwtFactoryService
    {
        public Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, List<Claim> additionalClaims);
        public ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id);
    }
}
