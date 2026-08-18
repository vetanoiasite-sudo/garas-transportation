using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PosNumber")]
public partial class PosNumber
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Serial { get; set; }

    [Column("StoreID")]
    public int? StoreId { get; set; }

    [InverseProperty("PosNumber")]
    public virtual ICollection<MedicalDailyTreasuryBalance> MedicalDailyTreasuryBalances { get; set; } = new List<MedicalDailyTreasuryBalance>();

    [ForeignKey("StoreId")]
    [InverseProperty("PosNumbers")]
    public virtual InventoryStore Store { get; set; }
}
