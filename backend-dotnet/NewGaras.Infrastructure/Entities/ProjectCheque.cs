using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectCheque")]
public partial class ProjectCheque
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(500)]
    public string ClientName { get; set; }

    [Required]
    [StringLength(500)]
    public string SalesPersonName { get; set; }

    [Required]
    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(500)]
    public string ProjectNumber { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ChequeDate { get; set; }

    [Column("ChequeChashingStatusID")]
    public int? ChequeChashingStatusId { get; set; }

    public bool? IsCrossedCheque { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal ChequeAmount { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    [Required]
    [StringLength(500)]
    public string Bank { get; set; }

    [Required]
    [StringLength(500)]
    public string BankBranch { get; set; }

    public string RejectCause { get; set; }

    public string Notes { get; set; }

    public string AttachmentPath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? WithdrawDate { get; set; }

    public long? WithdrawedBy { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [StringLength(250)]
    public string ChequeNumber { get; set; }

    [Column("MaintenanceForID")]
    public long? MaintenanceForId { get; set; }

    [Column("MaintenanceOrderID")]
    public long? MaintenanceOrderId { get; set; }

    [ForeignKey("ChequeChashingStatusId")]
    [InverseProperty("ProjectCheques")]
    public virtual ChequeCashingStatus ChequeChashingStatus { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectChequeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("ProjectCheques")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("MaintenanceForId")]
    [InverseProperty("ProjectCheques")]
    public virtual MaintenanceFor MaintenanceFor { get; set; }

    [ForeignKey("MaintenanceOrderId")]
    [InverseProperty("ProjectCheques")]
    public virtual ManagementOfMaintenanceOrder MaintenanceOrder { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectChequeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectCheques")]
    public virtual Project Project { get; set; }

    [ForeignKey("WithdrawedBy")]
    [InverseProperty("ProjectChequeWithdrawedByNavigations")]
    public virtual User WithdrawedByNavigation { get; set; }
}
