using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
    public class AppConfigRepository(IUnitOfWork unitOfWork) : Repository<AppConfig>(unitOfWork), IAppConfigRepository
    {
    }
}
