namespace NewGaras.Infrastructure.Models.Supplier
{
    public class GetSupplierContactPerson
    {
        public long? ID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}