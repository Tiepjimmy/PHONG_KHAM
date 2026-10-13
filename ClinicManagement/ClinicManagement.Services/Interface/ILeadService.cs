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
    public interface ILeadService
    {
        public Task<IResults<LeadViewModel>> GetAll(LeadSearchRequest request);
        public Task<IResult<bool>> Save(LeadRequest request, string typeSave);
        public IResults<Leads> GetAllData();
        public Task<IResult<Leads>> GetById(Guid id);
        public IResults<Leads> GetAllDataByRegionId(Guid? id);
        public IResults<Leads> GetAllDataByAreaId(Guid? id);
        public IResults<Leads> GetAllDataByBranchId(Guid? id);
        public IResults<Leads> GetAllDataByDepartmentId(Guid? id);
        public IResults<Leads> GetAllDataByUserId(Guid? id);
        public IResults<Leads> GetAllDataByCustomerId(Guid? id);
    }
}
