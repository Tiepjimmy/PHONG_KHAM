using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.SearchRequest
{
    public class CustomerSearchRequest : BaseSearchRequest
    {
        public string? CustomerType { get; set; }
        public string? CifNo { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxNo { get; set; }
        public string? IdentityNumber { get; set; }
        public string? BusinessRegistrationNumber { get; set; }
        public Guid? CompanySizeId { get; set; }
        public Guid? IndustryId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? Districtd { get; set; }
        public Guid? WardId { get; set; }
        public Guid? SegmentId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? UserAssignId { get; set; }
    }
}
