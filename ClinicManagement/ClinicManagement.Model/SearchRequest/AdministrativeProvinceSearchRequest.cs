namespace ClinicManagement.Model.SearchRequest
{
    public class AdministrativeProvinceSearchRequest : BaseSearchRequest
    {
        public Guid? AdministrativeUnitId { get; set; }
        public Guid? AdministrativeRegionId { get; set; }

    }
}
