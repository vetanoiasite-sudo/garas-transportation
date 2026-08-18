using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VVehicle
{
    [Column("ID")]
    public long Id { get; set; }

    public long ClientId { get; set; }

    [Required]
    [StringLength(50)]
    public string PlatNumber { get; set; }

    public int BrandId { get; set; }

    [StringLength(50)]
    public string VehicleBrandName { get; set; }

    public int? ModelId { get; set; }

    [StringLength(50)]
    public string VehicleModelName { get; set; }

    [Required]
    [StringLength(50)]
    public string Year { get; set; }

    public int? ColorId { get; set; }

    [StringLength(50)]
    public string VehicleColorName { get; set; }

    public int? Doors { get; set; }

    public int? TransmissionId { get; set; }

    [StringLength(50)]
    public string VehicleTransmissionName { get; set; }

    public int? Cylinders { get; set; }

    public int? Power { get; set; }

    public int? WheelsDriveId { get; set; }

    [StringLength(50)]
    public string VehicleWheelsDriveName { get; set; }

    [Column("VIN")]
    [StringLength(50)]
    public string Vin { get; set; }

    [StringLength(50)]
    public string LicenseNumber { get; set; }

    public int? CountryId { get; set; }

    [StringLength(500)]
    public string CountryName { get; set; }

    public int? CityId { get; set; }

    [StringLength(500)]
    public string GovernorateName { get; set; }

    public int? IssuerId { get; set; }

    [StringLength(50)]
    public string VehicleIssuerName { get; set; }

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

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(500)]
    public string ClientName { get; set; }

    public int? BodyTypeId { get; set; }

    [StringLength(50)]
    public string VehicleBodyTypeName { get; set; }

    [StringLength(200)]
    public string PriceRate { get; set; }
}
