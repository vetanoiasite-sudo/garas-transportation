using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ContractLeaveSetting")]
public partial class ContractLeaveSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string HolidayName { get; set; }

    public string Notes { get; set; }

    public bool? Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? BalancePerMonth { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? BalancePerYear { get; set; }

    public bool? Archive { get; set; }

    [InverseProperty("AbsenceType")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [InverseProperty("ContractLeaveSetting")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployees { get; set; } = new List<ContractLeaveEmployee>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("ContractLeaveSettingCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("VacationType")]
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ContractLeaveSettingModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
