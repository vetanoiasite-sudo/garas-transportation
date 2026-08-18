using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class MaintenanceReportExpense
{
    [Key]
    public long Id { get; set; }

    public long MaintenanceReportId { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal Amount { get; set; }

    public int CurrencyId { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    public bool Approve { get; set; }

    public string FilePath { get; set; }

    [StringLength(250)]
    public string FileName { get; set; }

    [StringLength(50)]
    public string Fileextention { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long? ExpensisTypeId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MaintenanceReportExpenses")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("MaintenanceReportExpenses")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("ExpensisTypeId")]
    [InverseProperty("MaintenanceReportExpenses")]
    public virtual ExpensisType ExpensisType { get; set; }

    [ForeignKey("MaintenanceReportId")]
    [InverseProperty("MaintenanceReportExpenses")]
    public virtual MaintenanceReport MaintenanceReport { get; set; }
}
