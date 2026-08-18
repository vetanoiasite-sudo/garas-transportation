using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskExpensis")]
public partial class TaskExpensi
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string Note { get; set; }

    [Required]
    [StringLength(50)]
    public string Curruncy { get; set; }

    [StringLength(500)]
    public string ImgPath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    [Column("ExpensisTypeID")]
    public long ExpensisTypeId { get; set; }

    public bool? Approved { get; set; }

    public long? ApprovedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovedDate { get; set; }

    public bool? Billable { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("TaskExpensiApprovedByNavigations")]
    public virtual User ApprovedByNavigation { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskExpensiCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ExpensisTypeId")]
    [InverseProperty("TaskExpensis")]
    public virtual ExpensisType ExpensisType { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskExpensiModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskExpensis")]
    public virtual Task Task { get; set; }
}
