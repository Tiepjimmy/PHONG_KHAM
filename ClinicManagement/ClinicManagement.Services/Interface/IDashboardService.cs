using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Model;

namespace ClinicManagement.Services.Interface
{
    public interface IDashboardService
    {
        public IResult<DashboardModel> GetCount();
        public IResult<DashboardModel> GetListChartOptionsBar();
        public IResult<DashboardModel> GetListChartOptionsPolarArea();
    }
}
