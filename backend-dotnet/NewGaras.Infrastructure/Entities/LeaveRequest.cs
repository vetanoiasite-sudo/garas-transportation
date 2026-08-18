using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("LeaveRequest")]
public partial class LeaveRequest
{
    [Column("HrUserID")]
    public long HrUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime From { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime To { get; set; }

    [Column("VacationTypeID")]
    public int VacationTypeId { get; set; }

    public bool? FirstApproval { get; set; }

    public bool? SecondApproval { get; set; }

    public long? FirstApprovedBy { get; set; }

    public long? SecondApprovedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FirstApprovalDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SecondApprovalDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public long ModifiedBy { get; set; }

    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string AbsenceCause { get; set; }

    [StringLength(500)]
    public string FirstRejectionCause { get; set; }

    [StringLength(500)]
    public string SecondRejectionCause { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("LeaveRequestCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("FirstApprovedBy")]
    [InverseProperty("LeaveRequestFirstApprovedByNavigations")]
    public virtual User FirstApprovedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("LeaveRequests")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("LeaveRequestModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("SecondApprovedBy")]
    [InverseProperty("LeaveRequestSecondApprovedByNavigations")]
    public virtual User SecondApprovedByNavigation { get; set; }

    [ForeignKey("VacationTypeId")]
    [InverseProperty("LeaveRequests")]
    public virtual ContractLeaveSetting VacationType { get; set; }
}
