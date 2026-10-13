using Microsoft.Extensions.Options;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Option;
using ClinicManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace ClinicManagement.Services.Implement
{
    public class JwtFactoryService(IOptions<JwtIssuerOptions> jwtOptions) : IJwtFactoryService
    {
        private readonly JwtIssuerOptions _jwtOptions = jwtOptions.Value;

        public ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), [
                new Claim(ConstantHelper.JwtClaimIdentifiers.Id, id.ToString()),
                new Claim(ConstantHelper.JwtClaimIdentifiers.Rol, ConstantHelper.JwtClaims.ApiAccess)
            ]);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, List<Claim> additionalClaims)
        {
            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(ConstantHelper.JwtClaimIdentifiers.Rol),
                identity.FindFirst(ConstantHelper.JwtClaimIdentifiers.Id)
            ];
            if (additionalClaims != null && additionalClaims.Count > 0)
            {
                claims.AddRange(additionalClaims);
            }
            JwtSecurityToken jwt = new(
               issuer: _jwtOptions.Issuer,
               audience: _jwtOptions.Audience,
               claims: claims,
               notBefore: _jwtOptions.NotBefore,
               expires: _jwtOptions.Expiration,
               signingCredentials: _jwtOptions.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
