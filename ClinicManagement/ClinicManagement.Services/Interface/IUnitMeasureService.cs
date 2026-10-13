using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Services.Interface
{
	public interface IUnitMeasureService
    {
		public Task<IResults<UnitMeasureViewModel>> GetAll(UnitMeasureSearchRequest request);
        public Task<IResult<bool>> Save(UnitMeasureRequest request, string typeSave);
        public IResults<UnitMeasure> GetAllData();
        public Task<IResult<UnitMeasure>> GetById(Guid id);
	}
}
