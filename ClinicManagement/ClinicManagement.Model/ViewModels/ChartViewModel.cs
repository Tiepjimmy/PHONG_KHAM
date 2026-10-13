namespace ClinicManagement.Model.ViewModels
{
    public class ChartViewModel
    {
        public List<long> Data { get; set; } = [];
        public List<string> Categories { get; set; } = [];
        public List<string> Color { get; set; } = [];
    }
}
