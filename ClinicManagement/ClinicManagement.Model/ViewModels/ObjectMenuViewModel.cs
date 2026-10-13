namespace ClinicManagement.Model.ViewModels
{
    public class ObjectMenuViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? ParentName { get; set; }
        public string? MetaLink { get; set; }
        public string? IconName { get; set; }
        public long? LevelShow { get; set; }
        public long? OrderShow { get; set; }
        public string? DataTarget { get; set; }
        public string? AriaControls { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
