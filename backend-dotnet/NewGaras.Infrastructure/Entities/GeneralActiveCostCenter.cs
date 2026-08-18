using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class GeneralActiveCostCenter
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string CostCenterName { get; set; }

    [StringLength(50)]
    public string Category { get; set; }

    [Column("CategoryID")]
    public long? CategoryId { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SellingPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CumulativeCost { get; set; }

    public bool Active { get; set; }

    public bool Closed { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("GeneralActiveCostCenterCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("GeneralActiveCostCenterModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
