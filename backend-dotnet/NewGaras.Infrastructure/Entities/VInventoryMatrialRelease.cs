using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VInventoryMatrialRelease
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("FromInventoryStoreID")]
    public int FromInventoryStoreId { get; set; }

    [StringLength(50)]
    public string ToUserFirstName { get; set; }

    [StringLength(50)]
    public string ToUserLastName { get; set; }

    [Column("ToUserID")]
    public long ToUserId { get; set; }

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

    [StringLength(101)]
    public string ToUserName { get; set; }

    [Column("MatrialRequestID")]
    public long MatrialRequestId { get; set; }

    [Column("DepartmentID")]
    public int? DepartmentId { get; set; }

    [StringLength(500)]
    public string FromUserDepartment { get; set; }

    [StringLength(500)]
    public string ToUserDepartment { get; set; }

    [Column("FromUserFName")]
    [StringLength(50)]
    public string FromUserFname { get; set; }

    [Column("FromUserLName")]
    [StringLength(50)]
    public string FromUserLname { get; set; }

    [Column("RequestTypeID")]
    public long? RequestTypeId { get; set; }

    [StringLength(200)]
    public string RequestTypeName { get; set; }

    [Column("StoreKeeperID")]
    public long? StoreKeeperId { get; set; }
}
