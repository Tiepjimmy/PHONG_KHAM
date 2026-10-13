using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Data.Repositories
{
    public class CustomerRepository(IUnitOfWork unitOfWork) : Repository<Customer>(unitOfWork), ICustomerRepository
    {
    }
}
