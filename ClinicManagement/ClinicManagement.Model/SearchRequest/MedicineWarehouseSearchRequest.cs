namespace ClinicManagement.Model.SearchRequest
{
    public class MedicineWarehouseSearchRequest : BaseSearchRequest
    {
        public Guid? SupplierId { get; set; }
        public int Quantity { get; set; }
        public bool IsMain { get; set; }
        public int Expiry { get; set; }
    }
}
