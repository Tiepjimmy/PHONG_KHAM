using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.Entity
{
    public class Region : BaseEntity
    {
        public string? Code { get; set; }
        public string? CodeName { get; set; }
        public string? NameVN { get; set; }
        public string? NameEN { get; set; }
        public string? FullNameVN { get; set; }
        public string? FullNameEN { get; set; }
        public bool IsDefault { get; set; }
    }
}
