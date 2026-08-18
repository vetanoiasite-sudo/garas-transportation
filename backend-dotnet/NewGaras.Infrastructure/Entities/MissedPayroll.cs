using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
[Table("Missed_Payroll")]
public partial class MissedPayroll
{
    [Column("HrUserID")]
    public long? HrUserId { get; set; }
}
