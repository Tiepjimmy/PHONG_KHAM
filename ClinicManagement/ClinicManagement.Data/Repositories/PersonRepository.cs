using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class PersonRepository(IUnitOfWork unitOfWork) : Repository<Person>(unitOfWork), IPersonRepository
    {
	}
}
