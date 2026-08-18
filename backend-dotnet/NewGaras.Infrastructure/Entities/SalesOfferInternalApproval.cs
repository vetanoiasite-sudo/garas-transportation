using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesOfferInternalApproval")]
public partial class SalesOfferInternalApproval
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long SalesOfferId { get; set; }

    [Required]
    [StringLength(100)]
    public string Type { get; set; }

    public long? UserId { get; set; }

    public long? GroupId { get; set; }

    public long? ByUser { get; set; }

    [StringLength(100)]
    public string Reply { get; set; }

    public string Comment { get; set; }

    public DateOnly? Date { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("ByUser")]
    [InverseProperty("SalesOfferInternalApprovalByUserNavigations")]
    public virtual User ByUserNavigation { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("SalesOfferInternalApprovals")]
    public virtual Group Group { get; set; }

    [ForeignKey("SalesOfferId")]
    [InverseProperty("SalesOfferInternalApprovals")]
    public virtual SalesOffer SalesOffer { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SalesOfferInternalApprovalUsers")]
    public virtual User User { get; set; }
}
