using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Governorate")]
public partial class Governorate
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    public long CreatedBy { get; set; }

    [Column("OldID")]
    public int? OldId { get; set; }

    [InverseProperty("Governorate")]
    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    [InverseProperty("Governorate")]
    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    [InverseProperty("Governorate")]
    public virtual ICollection<ClientAddress> ClientAddresses { get; set; } = new List<ClientAddress>();

    [InverseProperty("Governorate")]
    public virtual ICollection<ClientConsultantAddress> ClientConsultantAddresses { get; set; } = new List<ClientConsultantAddress>();

    [ForeignKey("CountryId")]
    [InverseProperty("Governorates")]
    public virtual Country Country { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("Governorates")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Governorate")]
    public virtual ICollection<ProjectContactPerson> ProjectContactPeople { get; set; } = new List<ProjectContactPerson>();

    [InverseProperty("Governorate")]
    public virtual ICollection<SalesOfferLocation> SalesOfferLocations { get; set; } = new List<SalesOfferLocation>();

    [InverseProperty("Governorate")]
    public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; } = new List<SupplierAddress>();

    [InverseProperty("City")]
    public virtual ICollection<UserPatient> UserPatients { get; set; } = new List<UserPatient>();

    [InverseProperty("City")]
    public virtual ICollection<VehiclePerClient> VehiclePerClients { get; set; } = new List<VehiclePerClient>();
}
