namespace ClinicManagement.Model.SearchRequest
{
    public class AdministrativeDistrictSearchRequest : BaseSearchRequest
    {
        public Guid? AdministrativeProvinceId { get; set; }
        public Guid? AdministrativeUnitId { get; set; }
    }
}
