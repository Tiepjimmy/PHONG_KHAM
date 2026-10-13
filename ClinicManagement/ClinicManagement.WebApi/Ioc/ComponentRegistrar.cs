using Autofac;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Option;
using ClinicManagement.Services.Interface;
using System.Reflection;

namespace ClinicManagement.WebApi.Ioc
{
    public class ComponentRegistrar : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var assembly = Assembly.GetExecutingAssembly();
#pragma warning disable CS8601 // Possible null reference assignment.
            builder.RegisterAssemblyTypes([Assembly.GetAssembly(typeof(ILoginService))]).Where(x => x.IsClass && x.Name.EndsWith("Service")).As(t => t.GetInterfaces().Length != 0 ? t.GetInterfaces()[0] : t);
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            builder.RegisterAssemblyTypes([Assembly.GetAssembly(typeof(IDapperRepository))]).Where(x => x.IsClass && x.Name.EndsWith("Repository")).As(t => t.GetInterfaces().Length != 0 ? t.GetInterfaces()[0] : t);
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            builder.RegisterAssemblyTypes([Assembly.GetAssembly(typeof(JwtIssuerOptions))]).Where(x => x.IsClass && x.Name.EndsWith("Options")).As(t => t.GetInterfaces().Length != 0 ? t.GetInterfaces()[0] : t);
#pragma warning restore CS8601 // Possible null reference assignment.

            //----------------------Common-----------------------------------------------
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<DbContext>().As<DbContext>();
            builder.RegisterType<Data.Share.AuthenticatedService>().As<Data.Share.IAuthenticatedService>();
        }
    }
}
