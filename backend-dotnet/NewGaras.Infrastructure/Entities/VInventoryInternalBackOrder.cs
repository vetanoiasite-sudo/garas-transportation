using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryInternalBackOrder
{
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string OperationType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public int Revision { get; set; }

    [Column("FromID")]
    public long FromId { get; set; }

    [StringLength(101)]
    public string FromUser { get; set; }

    [Column("InventoryStoreID")]
    public int InventoryStoreId { get; set; }

    [StringLength(1000)]
    public string StoreName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecivingDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("FromUserDepartmentID")]
    public int? FromUserDepartmentId { get; set; }

    [StringLength(500)]
    public string FromUserDepartment { get; set; }

    public bool Custody { get; set; }

    [Column("StoreKeeperID")]
    public long? StoreKeeperId { get; set; }
}
