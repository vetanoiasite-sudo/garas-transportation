using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MedicalDailyTreasuryBalance")]
public partial class MedicalDailyTreasuryBalance
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OpeningBalance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ClosingBalance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalReceipts { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReservationAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Difference { get; set; }

    public bool IsOpeningBalance { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ClosingDate { get; set; }

    public long ModifiedBy { get; set; }

    public long? ReceivedFrom { get; set; }

    public int? PosNumberId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("MedicalDailyTreasuryBalances")]
    public virtual Client Client { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MedicalDailyTreasuryBalanceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MedicalDailyTreasuryBalanceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("PosNumberId")]
    [InverseProperty("MedicalDailyTreasuryBalances")]
    public virtual PosNumber PosNumber { get; set; }

    [ForeignKey("ReceivedFrom")]
    [InverseProperty("MedicalDailyTreasuryBalanceReceivedFromNavigations")]
    public virtual User ReceivedFromNavigation { get; set; }
}
