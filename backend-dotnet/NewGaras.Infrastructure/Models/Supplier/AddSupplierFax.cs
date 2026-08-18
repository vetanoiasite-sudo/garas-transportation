namespace NewGaras.Infrastructure.Models.Supplier
{
    public class AddSupplierFax
    {
        public long? ID { get; set; }
        public string Fax { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}