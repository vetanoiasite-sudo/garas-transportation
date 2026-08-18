using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Country")]
public partial class Country
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
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("OldID")]
    public int? OldId { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    [InverseProperty("Country")]
    public virtual ICollection<ClientAddress> ClientAddresses { get; set; } = new List<ClientAddress>();

    [InverseProperty("Country")]
    public virtual ICollection<ClientConsultantAddress> ClientConsultantAddresses { get; set; } = new List<ClientConsultantAddress>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("CountryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<Governorate> Governorates { get; set; } = new List<Governorate>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("CountryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<ProjectContactPerson> ProjectContactPeople { get; set; } = new List<ProjectContactPerson>();

    [InverseProperty("Country")]
    public virtual ICollection<SalesOfferLocation> SalesOfferLocations { get; set; } = new List<SalesOfferLocation>();

    [InverseProperty("Country")]
    public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; } = new List<SupplierAddress>();

    [InverseProperty("Country")]
    public virtual ICollection<UserPatient> UserPatients { get; set; } = new List<UserPatient>();

    [InverseProperty("Country")]
    public virtual ICollection<VehiclePerClient> VehiclePerClients { get; set; } = new List<VehiclePerClient>();
}
