using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Services.Interface
{
    public interface ICustomerService
    {
        public Task<IResults<CustomerViewModel>> GetAll(CustomerSearchRequest request);
        public Task<IResult<bool>> Save(CustomerRequest request, string typeSave);
        public IResults<Customer> GetAllData();
        public Task<IResult<Customer>> GetById(Guid id);
        public IResults<Customer> GetAllDataByRegionId(Guid? regionId);
        public IResults<Customer> GetAllDataByAreaId(Guid? areaId);
        public IResults<Customer> GetAllDataByBranchId(Guid? branchId);
        public IResults<Customer> GetAllDataByDepartmentId(Guid? departmentId);
        public IResults<Customer> GetAllDataByUserAssignId(Guid? userAssignId);

    }
}
