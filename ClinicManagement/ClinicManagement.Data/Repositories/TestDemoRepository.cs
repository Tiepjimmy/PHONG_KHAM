using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class TestDemoRepository(IUnitOfWork unitOfWork) : Repository<TestDemo>(unitOfWork), ITestDemoRepository
    {
	}
}
