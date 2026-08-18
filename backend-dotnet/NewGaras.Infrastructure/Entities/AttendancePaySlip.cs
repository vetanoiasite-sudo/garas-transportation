using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AttendancePaySlip")]
public partial class AttendancePaySlip
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("EmployeeUserID")]
    public long EmployeeUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PaySlipDate { get; set; }

    public int Rev { get; set; }

    public bool IsCompleted { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal OverTimeSum { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal DelaySum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedyBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column("NoOFWorkingDays")]
    public int NoOfworkingDays { get; set; }
}
