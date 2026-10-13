using Dapper;
using Microsoft.Extensions.Configuration;
using ClinicManagement.Data.Connection;
using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Common;
using ClinicManagement.Model.Helper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ClinicManagement.Data.Repositories
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
	public class UnitOfWork : IUnitOfWork
    {
        protected readonly IConfiguration _config;
        protected string _cnStr = ConfigurationCommon.Instance.GetConfig<string>(ConstantHelper.ConnectionStrings, ConstantHelper.MainConnectionString);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UnitOfWork(IConfiguration config, DatabaseContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _config = config;
            _cnStr = _config.GetConnectionString(ConnectionHelper.DatabaseName.MainConnectionString.ToString()) ?? string.Empty;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _cn = new SqlConnection(_cnStr);
            _context = context;
        }

        public string ConnectionString
        {
            get => _cnStr;
        }

        protected IDbConnection _cn;
        public IDbConnection Connection
        {
            get => _cn;
        }

        protected DatabaseContext _context;
        public DatabaseContext DbContext
        {
            get => _context;
        }

        protected IDbTransaction? _trans = null;
        public IDbTransaction Transaction
        {
#pragma warning disable CS8603 // Possible null reference return.
            get => _trans;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IDbTransaction> OpenTransaction()
        {
            if (_trans != null)
            {
                throw new Exception("A transaction is already open, you need to use a new DbContext for parallel job.");
            }
            if (_cn.State == ConnectionState.Closed)
            {
                if (_cn is not DbConnection)
                {
                    throw new Exception("Connection object does not support OpenAsync.");
                }
                await ((DbConnection)_cn).OpenAsync();
            }
            _trans = _cn.BeginTransaction();
            return _trans;
        }

        public async Task<IDbTransaction> OpenTransaction(IsolationLevel level)
        {
            if (_trans != null)
            {
                throw new Exception("A transaction is already open, you need to use a new DbContext for parallel job.");
            }
            if (_cn.State == ConnectionState.Closed)
            {
                if (_cn is not DbConnection)
                {
                    throw new Exception("Connection object does not support OpenAsync.");
                }
                await ((DbConnection)_cn).OpenAsync();
            }
            _trans = _cn.BeginTransaction(level);
            return _trans;
        }

        public void CommitTransaction(bool disposeTrans = true)
        {
            if (_trans == null)
            {
                throw new Exception("DB Transaction is not present.");
            }
            _trans.Commit();
            if (disposeTrans)
            {
                _trans.Dispose();
            }
            if (disposeTrans)
            {
                _trans = null;
            }
        }

        public void RollbackTransaction(bool disposeTrans = true)
        {
            if (_trans == null)
            {
                throw new Exception("DB Transaction is not present.");
            }
            _trans.Rollback();
            if (disposeTrans)
            {
                _trans.Dispose();
            }
            if (disposeTrans)
            {
                _trans = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _trans?.Dispose();
            _cn?.Close();
            _cn?.Dispose();
        }

        public virtual void BeginTransaction() => _context.Database.BeginTransaction();

        public virtual void Commit() => _context.Database.CommitTransaction();

        public virtual void Rollback() => _context.Database.RollbackTransaction();

        public virtual int SaveChanges() => _context.SaveChanges();

        public virtual Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        private IUserRepository? _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(this);
            }
        }

        private IDocumentFileRepository? _documentFileRepository;
        public IDocumentFileRepository DocumentFileRepository
        {
            get
            {
                return _documentFileRepository ??= new DocumentFileRepository(this);
            }
        }

        private IObjectMenuRepository _objectMenuRepository;
        public IObjectMenuRepository ObjectMenuRepository
        {
            get
            {
                return _objectMenuRepository ??= new ObjectMenuRepository(this);
            }
        }

        private IUserGroupRepository _userGroupRepository;
        public IUserGroupRepository UserGroupRepository
        {
            get
            {
                return _userGroupRepository ??= new UserGroupRepository(this);
            }
        }

        private IUserRoleRepository _userRoleRepository;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                return _userRoleRepository ??= new UserRoleRepository(this);
            }
        }

        private ILocationTrackingRepository _locationTrackingRepository;
        public ILocationTrackingRepository LocationTrackingRepository
        {
            get
            {
                return _locationTrackingRepository ??= new LocationTrackingRepository(this);
            }
        }

        private IAuditChangeRepository _auditChangeRepository;
        public IAuditChangeRepository AuditChangeRepository
        {
            get
            {
                return _auditChangeRepository ??= new AuditChangeRepository(this);
            }
        }

        private IUserClaimRepository _userClaimRepository;
        public IUserClaimRepository UserClaimRepository
        {
            get
            {
                return _userClaimRepository ??= new UserClaimRepository(this);
            }
        }

		//Custom
	 
        private IPatientDetailRepository _PatientDetailRepository;
        public IPatientDetailRepository PatientDetailRepository
        {
            get
            {
                return _PatientDetailRepository ??= new PatientDetailRepository(this);
            }
        }
	 
        private IMedicalExaminationRepository _MedicalExaminationRepository;
        public IMedicalExaminationRepository MedicalExaminationRepository
        {
            get
            {
                return _MedicalExaminationRepository ??= new MedicalExaminationRepository(this);
            }
        }
	 
        private IMedicineWarehouseRepository _MedicineWarehouseRepository;
        public IMedicineWarehouseRepository MedicineWarehouseRepository
        {
            get
            {
                return _MedicineWarehouseRepository ??= new MedicineWarehouseRepository(this);
            }
        }
	 
        private ISupplierRepository _SupplierRepository;
        public ISupplierRepository SupplierRepository
        {
            get
            {
                return _SupplierRepository ??= new SupplierRepository(this);
            }
        }
	 
        private IUnitMeasureRepository _UnitMeasureRepository;
        public IUnitMeasureRepository UnitMeasureRepository
        {
            get
            {
                return _UnitMeasureRepository ??= new UnitMeasureRepository(this);
            }
        }
	 
        private IAdministrativeDistrictRepository _AdministrativeDistrictRepository;
        public IAdministrativeDistrictRepository AdministrativeDistrictRepository
        {
            get
            {
                return _AdministrativeDistrictRepository ??= new AdministrativeDistrictRepository(this);
            }
        }
	 
        private IAdministrativeProvinceRepository _AdministrativeProvinceRepository;
        public IAdministrativeProvinceRepository AdministrativeProvinceRepository
        {
            get
            {
                return _AdministrativeProvinceRepository ??= new AdministrativeProvinceRepository(this);
            }
        }
	 
        private IAdministrativeRegionRepository _AdministrativeRegionRepository;
        public IAdministrativeRegionRepository AdministrativeRegionRepository
        {
            get
            {
                return _AdministrativeRegionRepository ??= new AdministrativeRegionRepository(this);
            }
        }
	 
        private IAdministrativeUnitRepository _AdministrativeUnitRepository;
        public IAdministrativeUnitRepository AdministrativeUnitRepository
        {
            get
            {
                return _AdministrativeUnitRepository ??= new AdministrativeUnitRepository(this);
            }
        }
	 
        private IAdministrativeWardRepository _AdministrativeWardRepository;
        public IAdministrativeWardRepository AdministrativeWardRepository
        {
            get
            {
                return _AdministrativeWardRepository ??= new AdministrativeWardRepository(this);
            }
        }
	 
        private IPersonRepository _PersonRepository;
        public IPersonRepository PersonRepository
        {
            get
            {
                return _PersonRepository ??= new PersonRepository(this);
            }
        }
	 
        private ITestDemoRepository _TestDemoRepository;
        public ITestDemoRepository TestDemoRepository
        {
            get
            {
                return _TestDemoRepository ??= new TestDemoRepository(this);
            }
        }
	 
        private IPatientRepository _PatientRepository;
        public IPatientRepository PatientRepository
        {
            get
            {
                return _PatientRepository ??= new PatientRepository(this);
            }
        }
        private IAppConfigRepository _AppConfigRepository;
        public IAppConfigRepository AppConfigRepository
        {
            get
            {
                return _AppConfigRepository ??= new AppConfigRepository(this);
            }
        }
        private ICustomerRepository _CustomerRepository;
        public ICustomerRepository CustomerRepository
        {
            get
            {
                return _CustomerRepository ??= new CustomerRepository(this);
            }
        }
        private IRegionRepository _RegionRepository;
        public IRegionRepository RegionRepository
        {
            get
            {
                return _RegionRepository ??= new RegionRepository(this);
            }
        }
        private IAreaRepository _AreaRepository;
        public IAreaRepository AreaRepository
        {
            get
            {
                return _AreaRepository ??= new AreaRepository(this);
            }
        }
        private IBranchRepository _BranchRepository;
        public IBranchRepository BranchRepository
        {
            get
            {
                return _BranchRepository ??= new BranchRepository(this);
            }
        }
        private IDepartmentRepository _DepartmentRepository;
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return _DepartmentRepository ??= new DepartmentRepository(this);
            }
        }
        private IEmployeeRepository _EmployeeRepository;
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                return _EmployeeRepository ??= new EmployeeRepository(this);
            }
        }
        private IProductRepository _ProductRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                return _ProductRepository ??= new ProductRepository(this);
            }
        }
        private ILeadRepository _LeadRepository;
        public ILeadRepository LeadRepository
        {
            get
            {
                return _LeadRepository ??= new LeadRepository(this);
            }
        }
        private IOpportunityRepository _OpportunityRepository;
        public IOpportunityRepository OpportunityRepository
        {
            get
            {
                return _OpportunityRepository ??= new OpportunityRepository(this);
            }
        }
    }
}
