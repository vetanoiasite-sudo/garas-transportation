using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRUserWarningStatus")]
public partial class HruserWarningStatus
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    public bool Active { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<HruserWarning> HruserWarnings { get; set; } = new List<HruserWarning>();
}
