namespace NewGaras.Infrastructure.Models.Supplier
{
    public class AddSupplierPaymentTerm
    {
        public long? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Percentage { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}