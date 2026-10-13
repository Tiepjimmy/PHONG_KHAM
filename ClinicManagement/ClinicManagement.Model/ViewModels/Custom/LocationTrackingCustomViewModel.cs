namespace ClinicManagement.Model.ViewModels.Custom
{
    public class LocationTrackingCustomViewModel : LocationTrackingViewModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
