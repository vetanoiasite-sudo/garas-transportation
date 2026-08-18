using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallationBOQ")]
public partial class ProjectInstallationBoq
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long? SalesOfferProductId { get; set; }

    public double Quantity { get; set; }

    [Column("ProjectInstallationID")]
    public long ProjectInstallationId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public bool Active { get; set; }

    [StringLength(50)]
    public string ItemSerial { get; set; }

    public bool CertificateOfGuaranteeStatus { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationBoqCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectInstallationBoqModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectInstallationId")]
    [InverseProperty("ProjectInstallationBoqs")]
    public virtual ProjectInstallation ProjectInstallation { get; set; }

    [ForeignKey("SalesOfferProductId")]
    [InverseProperty("ProjectInstallationBoqs")]
    public virtual SalesOfferProduct SalesOfferProduct { get; set; }
}
