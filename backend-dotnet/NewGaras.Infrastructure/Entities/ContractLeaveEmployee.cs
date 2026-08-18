using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ContractLeaveEmployee")]
public partial class ContractLeaveEmployee
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    [Column("ContractID")]
    public long ContractId { get; set; }

    [Column("ContractLeaveSettingID")]
    public int ContractLeaveSettingId { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string LeaveAllowed { get; set; }

    public int? Balance { get; set; }

    public int? Used { get; set; }

    public int? Remain { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    public long? HrUserId { get; set; }

    public string Comment { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? BalancePerMonth { get; set; }

    [ForeignKey("ContractId")]
    [InverseProperty("ContractLeaveEmployees")]
    public virtual ContractDetail Contract { get; set; }

    [ForeignKey("ContractLeaveSettingId")]
    [InverseProperty("ContractLeaveEmployees")]
    public virtual ContractLeaveSetting ContractLeaveSetting { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ContractLeaveEmployeeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("ContractLeaveEmployees")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ContractLeaveEmployeeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ContractLeaveEmployeeUsers")]
    public virtual User User { get; set; }
}
