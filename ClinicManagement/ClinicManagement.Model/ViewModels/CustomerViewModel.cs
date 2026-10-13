using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Model.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public string? CustomerType { get; set; }
        public string? StatusId { get; set; }
        public string? CifNo { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string? IdentityPlace { get; set; }
        public string? Occupation { get; set; }
        public string? LegalName { get; set; }
        public string? BusinessRegistrationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistrationPlace { get; set; }
        public Guid? CompanySizeId { get; set; }
        public Guid? IndustryId { get; set; }
        public string? LegalRepresentativeName { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhone { get; set; }
        public string? ContactPersonEmail { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? Districtd { get; set; }
        public Guid? WardId { get; set; }
        public string? Address { get; set; }
        public Guid? SegmentId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? UserAssignId { get; set; }
        public string? Note { get; set; }
        public string? ExternalData { get; set; }
        public string? ExternalValue { get; set; }
    }
}
