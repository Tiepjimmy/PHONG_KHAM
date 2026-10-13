using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.SearchRequest
{
    public class BranchSearchRequest : BaseSearchRequest
    {
        public Guid? RegionId { get; set; }
        public Guid? AreaId { get; set; }
    }
}
