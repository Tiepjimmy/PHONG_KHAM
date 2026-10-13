using ClinicManagement.Data.Connection;
using ClinicManagement.Model.Helper;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.WebApi.Infrastructure
{
    public static class DatabaseConnection
    {
        public static void InitConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionHelper.DatabaseName.MainConnectionString.ToString()));
            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }
    }
}
