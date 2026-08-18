namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SupplierMainData
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool? HasLogo { get; set; }
        public string Status { get; set; }
        public string ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public int RemainingDays { get; set; }
        public string RegistrationDate { get; set; }
        public decimal RemainCollection {  get; set; }
        public decimal Volume { get; set; }
        public long? SupplierSerialCounter { get; set; }
        public string SupplierSerial {  get; set; }

        public List<Vehicle> SupplierVehicles { get; set; }
    }
}