using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.Request
{
    public class ProductRequest : BaseRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? LevelData { get; set; }
        public bool? isDisplay { get; set; }
        public int? Sort { get; set; }
        public Guid? ParentId { get; set; }
        public string? Note { get; set; }
        public string? ExternalData { get; set; }
        public string? ExternalValue { get; set; }
    }
}
