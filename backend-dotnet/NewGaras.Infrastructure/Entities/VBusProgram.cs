using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VBusProgram
{
    [Column("Employee_Number")]
    [StringLength(500)]
    public int EmployeeNumber { get; set; }

    [Column("Emp_Name_Ara")]
    [StringLength(500)]
    public string EmpNameAra { get; set; }

    [StringLength(500)]
    public string BusType { get; set; }

    [Column("TYPE")]
    [StringLength(500)]
    public string Type { get; set; }

    [Column("Check_Date")]
    [StringLength(500)]
    public string CheckDate { get; set; }

    [Column("Check_time")]
    [StringLength(500)]
    public string CheckTime { get; set; }
}
