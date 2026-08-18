using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EmailType")]
public partial class EmailType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string TypeName { get; set; }

    [InverseProperty("EmailTypeNavigation")]
    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
}
