using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Services.Interface
{
    public interface IAppConfigService
    {
        public Task<IResults<AppConfigViewModel>> GetAll(AppConfigSearchRequest request);
        public Task<IResult<bool>> Save(AppConfigRequest request, string typeSave);
        public IResults<AppConfig> GetAllData();
        public Task<IResult<AppConfig>> GetById(Guid id);
    }
}
