using AutoMapper;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Request;

namespace ClinicManagement.WebApi.Infrastructure
{
	public class AutoMapperConfiguraion : Profile
    {
		public AutoMapperConfiguraion()
        {
            CreateMap<ObjectMenuRequest, ObjectMenu>();
            CreateMap<UserGroupRequest, UserGroup>();
            CreateMap<UserRequest, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<LocationTrackingRequest, LocationTracking>();
            CreateMap<AuditChangeRequest, AuditChange>();
            CreateMap<UserClaimRequest, UserClaim>();
			//Custom
			CreateMap<PatientDetailRequest, PatientDetail>();
			CreateMap<MedicalExaminationRequest, MedicalExamination>();
			CreateMap<MedicineWarehouseRequest, MedicineWarehouse>();
			CreateMap<SupplierRequest, Supplier>();
			CreateMap<UnitMeasureRequest, UnitMeasure>();
			CreateMap<AdministrativeDistrictRequest, AdministrativeDistrict>();
			CreateMap<AdministrativeProvinceRequest, AdministrativeProvince>();
			CreateMap<AdministrativeRegionRequest, AdministrativeRegion>();
			CreateMap<AdministrativeUnitRequest, AdministrativeUnit>();
			CreateMap<AdministrativeWardRequest, AdministrativeWard>();
			CreateMap<PersonRequest, Person>();
			CreateMap<TestDemoRequest, TestDemo>();
			CreateMap<PatientRequest, Patient>();
			//App
            CreateMap<AppConfigRequest, AppConfig>();
            CreateMap<CustomerRequest, Customer>();
            CreateMap<RegionRequest, Region>();
            CreateMap<AreaRequest, Area>();
            CreateMap<BranchRequest, Branch>();
            CreateMap<DepartmentRequest, Department>();
            CreateMap<EmployeeRequest, Employee>();
            CreateMap<ProductRequest, Products>();
            CreateMap<LeadRequest, Leads>();
            CreateMap<OpportunityRequest, Opportunity>();
        }
    }
}
