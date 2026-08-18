namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SupplierConsultantData
    {
        public long? Id { get; set; }
        public long SupplierId { get; set; }
        public string ConsultantName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Company {  get; set; }
        public string ConsultantFor { get; set; }

       
    }
}