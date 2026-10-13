namespace ClinicManagement.Model.ViewModels
{
	public class MedicineWarehouseViewModel : BaseViewModel
    {
		public string? Name { get; set; }
		public int Quantity { get; set; }
		public Guid? SupplierId { get; set; }
		public string? SupplierName { get; set; }
		public Guid? UnitMeasureId { get; set; }
		public string? UnitMeasureName { get; set; }
		public decimal PriceInput { get; set; }
		public DateTime? DateInput { get; set; }
		public decimal Price { get; set; }
		public DateTime? Expiry { get; set; }
		public decimal PriceTotal { get; set; }
		public string? Note { get; set; }
		public bool IsMain { get; set; }
	}
}
