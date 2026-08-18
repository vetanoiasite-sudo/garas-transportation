using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPurchaseRequest
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("ToUserID")]
    public long? ToUserId { get; set; }

    [StringLength(50)]
    public string ToUserFirstName { get; set; }

    [StringLength(50)]
    public string ToUserLastName { get; set; }

    [Column("FromInventoryStoreID")]
    public int FromInventoryStoreId { get; set; }

    [StringLength(1000)]
    public string FromInventoryStoreName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("MatrialRequestID")]
    public long MatrialRequestId { get; set; }

    [Column("IsDirectPR")]
    public bool? IsDirectPr { get; set; }

    [StringLength(50)]
    public string ApprovalStatus { get; set; }

    [Column("ApprovalUserID")]
    public long? ApprovalUserId { get; set; }

    [StringLength(500)]
    public string ApprovalReplyNotes { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovalReplyData { get; set; }
}
