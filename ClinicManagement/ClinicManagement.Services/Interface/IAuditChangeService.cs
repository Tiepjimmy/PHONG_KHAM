using ClinicManagement.Data.Interface;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Model.ViewModels.Custom;

namespace ClinicManagement.Services.Interface
{
    public interface IAuditChangeService
    {
        public Task<IResults<AuditChangeCustomViewModel>> GetAll(AuditChangeSearchRequest request);
        public Task<IResults<AuditChangeViewModel>> GetAllEntityName();
        public IResults<AuditChangeViewModel> GetAllAction();
        public Task<IResult<bool>> DeleteData();
        public Task<IResults<ColumnNameForeignKeyViewModel>> GetColumnNameForeignKey();
    }
}
