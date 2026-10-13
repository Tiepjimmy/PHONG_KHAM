using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
    public interface IMedicalExaminationService
    {
        public Task<IResults<MedicalExaminationViewModel>> GetAll(MedicalExaminationSearchRequest request);
        public Task<IResult<bool>> Save(MedicalExaminationRequest request, string typeSave);
        public IResults<MedicalExamination> GetAllData();
        public Task<IResult<MedicalExamination>> GetById(Guid id);
    }
}
