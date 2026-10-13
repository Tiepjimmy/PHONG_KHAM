namespace ClinicManagement.Model.Entity
{
    public class ObjectMenu : BaseEntity
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public Guid? ParentId { get; set; }
        public string? MetaLink { get; set; }
        public string? IconName { get; set; }
        public int? LevelShow { get; set; }
        public int? OrderShow { get; set; }
        public string? DataTarget { get; set; }
        public string? AriaControls { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
