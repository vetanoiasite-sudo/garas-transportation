using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EmailCc")]
public partial class EmailCc
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Email { get; set; }

    public bool Active { get; set; }

    [Column("EmailID")]
    public long EmailId { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("EmailCcs")]
    public virtual Email EmailNavigation { get; set; }
}
