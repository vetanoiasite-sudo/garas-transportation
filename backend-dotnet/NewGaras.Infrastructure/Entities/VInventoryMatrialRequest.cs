using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialRequest
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("FromUserID")]
    public long FromUserId { get; set; }

    [StringLength(50)]
    public string FromUserFirstName { get; set; }

    [StringLength(50)]
    public string FromUserLastName { get; set; }

    [Column("ToInventoryStoreID")]
    public int ToInventoryStoreId { get; set; }

    [StringLength(1000)]
    public string ToInventoryStoreName { get; set; }

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

    [StringLength(101)]
    public string FromUserName { get; set; }

    [Column("RequestTypeID")]
    public long? RequestTypeId { get; set; }

    [StringLength(200)]
    public string RequestType { get; set; }

    [StringLength(50)]
    public string ApproveResult { get; set; }

    public string ApproveRejectNotes { get; set; }

    [Column("StoreKeeperID")]
    public long? StoreKeeperId { get; set; }
}
