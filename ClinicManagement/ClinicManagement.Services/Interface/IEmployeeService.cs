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
    public interface IEmployeeService
    {
        public Task<IResults<EmployeeViewModel>> GetAll(EmployeeSearchRequest request);
        public Task<IResult<bool>> Save(EmployeeRequest request, string typeSave);
        public IResults<Employee> GetAllData();
        public Task<IResult<Employee>> GetById(Guid id);
        public IResults<Employee> GetAllDataByRegionId(Guid? id);
        public IResults<Employee> GetAllDataByAreaId(Guid? id);
        public IResults<Employee> GetAllDataByBranchId(Guid? id);
        public IResults<Employee> GetAllDataByDepartmentId(Guid? id);
    }
}
