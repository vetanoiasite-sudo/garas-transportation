using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehiclePerClient")]
public partial class VehiclePerClient
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long ClientId { get; set; }

    [Required]
    [StringLength(50)]
    public string PlatNumber { get; set; }

    public int BrandId { get; set; }

    public int? ModelId { get; set; }

    public int? BodyTypeId { get; set; }

    [Required]
    [StringLength(50)]
    public string Year { get; set; }

    public int? ColorId { get; set; }

    public int? Doors { get; set; }

    public int? TransmissionId { get; set; }

    public int? Cylinders { get; set; }

    public int? Power { get; set; }

    public int? WheelsDriveId { get; set; }

    [Column("VIN")]
    [StringLength(50)]
    public string Vin { get; set; }

    [StringLength(50)]
    public string LicenseNumber { get; set; }

    public int? CountryId { get; set; }

    public int? CityId { get; set; }

    public int? IssuerId { get; set; }

    public int? Odometer { get; set; }

    [StringLength(1000)]
    public string LicenseAttachmentPath { get; set; }

    [StringLength(250)]
    public string LicenseFileName { get; set; }

    [StringLength(5)]
    public string LicenseFileExtenssion { get; set; }

    [StringLength(50)]
    public string ChassisNumber { get; set; }

    [StringLength(50)]
    public string MotorNumber { get; set; }

    [StringLength(1000)]
    public string VehicleAttachmentPath { get; set; }

    [StringLength(250)]
    public string VehicleFileName { get; set; }

    [StringLength(5)]
    public string VehicleFileExtenssion { get; set; }

    [StringLength(200)]
    public string PriceRate { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BodyTypeId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleBodyType BodyType { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleBrand Brand { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("VehiclePerClients")]
    public virtual Governorate City { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("VehiclePerClients")]
    public virtual Client Client { get; set; }

    [ForeignKey("ColorId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleColor Color { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("VehiclePerClients")]
    public virtual Country Country { get; set; }

    [ForeignKey("IssuerId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleIssuer Issuer { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleModel Model { get; set; }

    [ForeignKey("TransmissionId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleTransmission Transmission { get; set; }

    [InverseProperty("VehiclePerClient")]
    public virtual ICollection<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistories { get; set; } = new List<VehicleMaintenanceJobOrderHistory>();

    [ForeignKey("WheelsDriveId")]
    [InverseProperty("VehiclePerClients")]
    public virtual VehicleWheelsDrive WheelsDrive { get; set; }
}
