namespace ClinicManagement.Model.Entity
{
	public class UserRole : BaseEntity
    {
		public Guid? UserGroupId { get; set; }
		public Guid? ObjectMenuId { get; set; }
		public bool AllowAccess { get; set; }
		public bool AllowAdd { get; set; }
		public bool AllowEdit { get; set; }
		public bool AllowDelete { get; set; }
		public bool AllowPrint { get; set; }
		public bool AllowExport { get; set; }
		public bool AllowImport { get; set; }
	}
}
