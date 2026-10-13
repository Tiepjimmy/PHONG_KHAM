using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ClinicManagement.Data.Share;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using System.Text;

namespace ClinicManagement.Data.Connection
{
	public class DatabaseContext(DbContextOptions<DatabaseContext> options, IAuthenticatedService authenticatedService) : IdentityDbContext(options)
    {
        private readonly IAuthenticatedService _authenticatedService = authenticatedService;

		public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ObjectMenu> ObjectMenu { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<DocumentFile> DocumentFile { get; set; }
        public virtual DbSet<LocationTracking> LocationTracking { get; set; }
        public virtual DbSet<AuditChange> AuditChange { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
		//Custom
		public virtual DbSet<PatientDetail> PatientDetail { get; set; }
		public virtual DbSet<MedicalExamination> MedicalExamination { get; set; }
		public virtual DbSet<MedicineWarehouse> MedicineWarehouse { get; set; }
		public virtual DbSet<Supplier> Supplier { get; set; }
		public virtual DbSet<UnitMeasure> UnitMeasure { get; set; }
		public virtual DbSet<AdministrativeDistrict> AdministrativeDistrict { get; set; }
		public virtual DbSet<AdministrativeProvince> AdministrativeProvince { get; set; }
		public virtual DbSet<AdministrativeRegion> AdministrativeRegion { get; set; }
		public virtual DbSet<AdministrativeUnit> AdministrativeUnit { get; set; }
		public virtual DbSet<AdministrativeWard> AdministrativeWard { get; set; }
		public virtual DbSet<Person> Person { get; set; }
		public virtual DbSet<TestDemo> TestDemo { get; set; }
		public virtual DbSet<Patient> Patient { get; set; }
        //App
        public virtual DbSet<AppConfig> AppConfig { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Leads> Leads { get; set; }
        public virtual DbSet<Opportunity> Opportunity { get; set; }

        public async Task<int> SaveChangesAsync(string typeSave, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = _authenticatedService.Id;
            var date = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = date;
                        entry.Entity.CreatedBy = user;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        if (typeSave == ConstantHelper.TypeSave.Update || typeSave == ConstantHelper.TypeSave.UpdateProfile)
                        {
                            entry.Entity.ModifiedDate = date;
                            entry.Entity.ModifiedBy = user;
                        }
                        if (typeSave == ConstantHelper.TypeSave.Delete)
                        {
                            entry.Entity.DeletedDate = date;
                            entry.Entity.DeleteddBy = user;
                            entry.Entity.IsDeleted = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            var modifiedEntities = ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var modifiedEntity in modifiedEntities)
            {
                AuditChange auditLog = new()
                {
                    IdChange = modifiedEntity.Entity.Id,
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    CreatedDate = date,
                    CreatedBy = user,
                    Changes = GetChanges(modifiedEntity, modifiedEntity.State),
                    IsDeleted = false,
                };
                AuditChange.Add(auditLog);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        private static string GetChanges(EntityEntry entity, EntityState state)
        {
            StringBuilder changes = new();
            foreach (var property in entity.OriginalValues.Properties)
            {
                var originalValue = state == EntityState.Modified ? entity.OriginalValues[property] : ConstantHelper.Space;
                var currentValue = entity.CurrentValues[property];
                if (!Equals(originalValue, currentValue))
                {
                    if (state == EntityState.Added && !string.IsNullOrEmpty(currentValue?.ToString()?.Trim()))
                    {
                        changes.AppendLine($"- {property.Name}: '{currentValue}' {ConstantHelper.Break}");
                    }
                    if (state == EntityState.Modified)
                    {
                        changes.AppendLine($"- {property.Name}: From '{originalValue}' to '{currentValue}' {ConstantHelper.Break}");
                    }
                }
            }
            return changes.ToString();
        }
    }
}
