using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesTarget")]
public partial class SalesTarget
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int Year { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FromDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ToDate { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Target { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public bool Active { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    public long? Modified { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public bool CanEdit { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesTargetCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("Modified")]
    [InverseProperty("SalesTargetModifiedNavigations")]
    public virtual User ModifiedNavigation { get; set; }

    [InverseProperty("Target")]
    public virtual ICollection<SalesBranchProductTarget> SalesBranchProductTargets { get; set; } = new List<SalesBranchProductTarget>();

    [InverseProperty("Target")]
    public virtual ICollection<SalesBranchTarget> SalesBranchTargets { get; set; } = new List<SalesBranchTarget>();

    [InverseProperty("Target")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargets { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("Target")]
    public virtual ICollection<SalesBranchUserTarget> SalesBranchUserTargets { get; set; } = new List<SalesBranchUserTarget>();
}
