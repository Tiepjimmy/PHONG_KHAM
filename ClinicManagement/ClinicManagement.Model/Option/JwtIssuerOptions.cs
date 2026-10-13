using Microsoft.IdentityModel.Tokens;

namespace ClinicManagement.Model.Option
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public class JwtIssuerOptions
    {
        public string? Issuer { get; set; }
        public string? Subject { get; set; }
        public string? Audience { get; set; }
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public DateTime NotBefore => DateTime.Now;
        public DateTime IssuedAt => DateTime.Now;
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);
        public string? Secret => "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials? SigningCredentials { get; set; }
    }
}
