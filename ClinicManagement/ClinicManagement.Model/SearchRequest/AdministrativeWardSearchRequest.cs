namespace ClinicManagement.Model.SearchRequest
{
    public class AdministrativeWardSearchRequest : BaseSearchRequest
    {
        public Guid? AdministrativeDistrictId { get; set; }
        public Guid? AdministrativeUnitId { get; set; }
    }
}
