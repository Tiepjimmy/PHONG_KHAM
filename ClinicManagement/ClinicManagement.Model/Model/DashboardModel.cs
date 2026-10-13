using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Model.Model
{
    public class DashboardModel
    {
        //Chart
        public ChartViewModel? ChartOptionsBar { get; set; }
        public ChartViewModel? ChartOptionsPolarArea { get; set; }
    }
}
