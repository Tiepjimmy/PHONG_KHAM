using ClinicManagement.Data.Connection;
using System.Data;

namespace ClinicManagement.Data.Interface
{
	public interface IUnitOfWork : IDisposable
    {
		public IDbConnection Connection { get; }
		public DatabaseContext DbContext { get; }
        public IDbTransaction Transaction { get; }
        public string ConnectionString { get; }
        public Task<IDbTransaction> OpenTransaction();
        public Task<IDbTransaction> OpenTransaction(IsolationLevel level);
        public void CommitTransaction(bool disposeTrans = true);
        public void RollbackTransaction(bool disposeTrans = true);
        public void BeginTransaction();
        public int SaveChanges();
        public void Commit();
        public void Rollback();
        public Task<int> SaveChangesAsync();
        public IUserRepository UserRepository { get; }
        public IDocumentFileRepository DocumentFileRepository { get; }
        public ILocationTrackingRepository LocationTrackingRepository { get; }
        public IObjectMenuRepository ObjectMenuRepository { get; }
        public IUserGroupRepository UserGroupRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IAuditChangeRepository AuditChangeRepository { get; }
        public IUserClaimRepository UserClaimRepository { get; }
		//Custom
		public IPatientDetailRepository PatientDetailRepository { get; }
		public IMedicalExaminationRepository MedicalExaminationRepository { get; }
		public IMedicineWarehouseRepository MedicineWarehouseRepository { get; }
		public ISupplierRepository SupplierRepository { get; }
		public IUnitMeasureRepository UnitMeasureRepository { get; }
		public IAdministrativeDistrictRepository AdministrativeDistrictRepository { get; }
		public IAdministrativeProvinceRepository AdministrativeProvinceRepository { get; }
		public IAdministrativeRegionRepository AdministrativeRegionRepository { get; }
		public IAdministrativeUnitRepository AdministrativeUnitRepository { get; }
		public IAdministrativeWardRepository AdministrativeWardRepository { get; }
		public IPersonRepository PersonRepository { get; }
		public ITestDemoRepository TestDemoRepository { get; }
		public IPatientRepository PatientRepository { get; }
        public IAppConfigRepository AppConfigRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IRegionRepository RegionRepository { get; }
        public IAreaRepository AreaRepository { get; }
        public IBranchRepository BranchRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ILeadRepository LeadRepository { get; }
        public IOpportunityRepository OpportunityRepository { get; }
    }
}
