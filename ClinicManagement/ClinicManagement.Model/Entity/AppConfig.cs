using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.Entity
{
    public class AppConfig : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public Guid? ParentId { get; set; }
        public bool? isDisplay { get; set; }
        public int? Sort { get; set; }
        public string? Note { get; set; }
    }
}
