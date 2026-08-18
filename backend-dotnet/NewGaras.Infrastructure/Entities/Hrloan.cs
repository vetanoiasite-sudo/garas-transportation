using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRLoan")]
public partial class Hrloan
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LoanAmount { get; set; }

    [Column("ApprovedByUserID")]
    public long? ApprovedByUserId { get; set; }

    [Column("ApprovalStatusID")]
    public int? ApprovalStatusId { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LoanDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RefundDate { get; set; }

    public string SignedMemoAttachmentPath { get; set; }

    [StringLength(50)]
    public string SignedMemoFileName { get; set; }

    [StringLength(50)]
    public string SignedMemoFileExtension { get; set; }

    [Column("LoanStatusID")]
    public int LoanStatusId { get; set; }

    [Column("RefundStrategyID")]
    public int RefundStrategyId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReturnedAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? RemainAmount { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    public int ForMonths { get; set; }

    [ForeignKey("ApprovalStatusId")]
    [InverseProperty("Hrloans")]
    public virtual HrloanApprovalStatus ApprovalStatus { get; set; }

    [ForeignKey("ApprovedByUserId")]
    [InverseProperty("HrloanApprovedByUsers")]
    public virtual User ApprovedByUser { get; set; }

    [ForeignKey("LoanStatusId")]
    [InverseProperty("Hrloans")]
    public virtual HrloanStatus LoanStatus { get; set; }

    [ForeignKey("RefundStrategyId")]
    [InverseProperty("Hrloans")]
    public virtual HrloanRefundStrategy RefundStrategy { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("HrloanUsers")]
    public virtual User User { get; set; }
}
