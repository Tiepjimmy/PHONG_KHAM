using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.Entity
{
    public class Opportunity : BaseEntity
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CifNo { get; set; }
        public string? TaxNo { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid? ProductGroupId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? ChannelId { get; set; }
        public Guid? FlowId { get; set; }
        public Guid? StageId { get; set; }
        public Guid? StageStatusId { get; set; }
        public Guid? ResultId { get; set; }
        public Guid? ReaonId { get; set; }
        public string? ReaonNote { get; set; }
        public Guid? NextStepId { get; set; }
        public Guid? StatusId { get; set; }
        public string? ExpectedValue { get; set; }
        public string? ExpectedTime { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsDuplicate { get; set; }
        public string? LeadScoring { get; set; }
        public string? Note { get; set; }
        public string? Purpose { get; set; }
        public string? ExternalData { get; set; }
        public string? ExternalValue { get; set; }
    }
}
