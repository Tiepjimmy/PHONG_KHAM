using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Option;
using System.Text;

namespace ClinicManagement.WebApi.Infrastructure
{
    public static class ConfigureService
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
        private static readonly SymmetricSecurityKey SigningKey = new(Encoding.ASCII.GetBytes(SecretKey));

        public static void InitAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration = ServiceLocator.Current.GetInstance<IConfiguration>();
            // Register the ConfigurationBuilder instance of FacebookAuthSetting
            //services.Configure<FacebookAuthSettings>(configuration.GetSection(nameof(FacebookAuthSettings)));
            // Get options from app settings
            IConfigurationSection jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
            string? a = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)] ?? string.Empty;
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)] ?? string.Empty;
                options.SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            });

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });
            // api user claim policy
            services.AddAuthorizationBuilder().AddPolicy("ApiUser", policy => policy.RequireClaim(ConstantHelper.JwtClaimIdentifiers.Rol, ConstantHelper.JwtClaims.ApiAccess));

            IConfigurationSection appSettingOptions = configuration.GetSection(nameof(AppSettingHelper));
            services.Configure<AppSettingHelper>(options =>
            {
                options.DomainFile = appSettingOptions[nameof(AppSettingHelper.DomainFile)] ?? string.Empty;
            });
        }
    }
}
