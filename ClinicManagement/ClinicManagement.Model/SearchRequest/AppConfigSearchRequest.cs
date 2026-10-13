using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.SearchRequest
{
    public class AppConfigSearchRequest : BaseSearchRequest
    {
        public string? ParentId { get; set; }
    }
}
