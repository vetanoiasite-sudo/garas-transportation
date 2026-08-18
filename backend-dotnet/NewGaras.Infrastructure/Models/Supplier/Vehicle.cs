
namespace NewGaras.Infrastructure.Models.Supplier
{
    public class Vehicle
    {
        public long? Id { get; set; }
        public string PlateNumber { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int? ModelId { get; set; }
        public string ModelName { get; set; }
        public int? BodyTypeId { get; set; }
        public string BodyTypeName { get; set; }
        public string Year { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public int? Doors { get; set; }
        public int? TransmissionId { get; set; }
        public string TransmissionName { get; set; }
        public int? Cylinders { get; set; }
        public int? Power { get; set; }
        public int? WheelsDriveId { get; set; }
        public string WheelsDriveName { get; set; }
        public string VIN { get; set; }
        public string LicenseNumber { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? IssuerId { get; set; }
        public string IssuerName { get; set; }
        public int? Odometer { get; set; }
        public string ChassisNumber { get; set; }
        public string MotorNumber { get; set; }
        public string PriceRate { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}