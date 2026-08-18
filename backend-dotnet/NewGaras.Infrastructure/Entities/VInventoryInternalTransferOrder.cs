using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryInternalTransferOrder
{
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string OperationType { get; set; }

    public int Revision { get; set; }

    [Column("FromInventoryStoreID")]
    public int FromInventoryStoreId { get; set; }

    [Column("ToInventoryStoreID")]
    public int ToInventoryStoreId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecivingDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(101)]
    public string CreatorUserName { get; set; }

    [Required]
    [StringLength(1000)]
    public string FromInventoryStoreName { get; set; }

    [Column("FromInventoryKeeperUserID")]
    public long FromInventoryKeeperUserId { get; set; }

    [Required]
    [StringLength(1000)]
    public string ToInventoryStoreName { get; set; }

    [Column("ToInventoryKeeperUserID")]
    public long ToInventoryKeeperUserId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
}
