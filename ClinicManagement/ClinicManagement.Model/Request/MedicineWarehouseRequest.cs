namespace ClinicManagement.Model.Request
{
	public class MedicineWarehouseRequest : BaseRequest
    {
		public string? Name { get; set; }
		public int Quantity { get; set; }
		public Guid? SupplierId { get; set; }
		public Guid? UnitMeasureId { get; set; }
		public decimal PriceInput { get; set; }
		public DateTime? DateInput { get; set; }
		public decimal Price { get; set; }
		public DateTime? Expiry { get; set; }
		public decimal PriceTotal { get; set; }
		public string? Note { get; set; }
		public bool IsMain { get; set; }
	}
}
