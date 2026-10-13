using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.ViewModels
{
    public class AreaViewModel : BaseViewModel
    {
        public string? Code { get; set; }
        public string? CodeName { get; set; }
        public string? NameVN { get; set; }
        public string? NameEN { get; set; }
        public string? FullNameVN { get; set; }
        public string? FullNameEN { get; set; }
        public Guid? RegionId { get; set; }
        public string? RegionName { get; set; }
        public bool IsDefault { get; set; }
    }
}
