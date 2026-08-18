using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class EmailReceiver
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Email { get; set; }

    public bool Active { get; set; }

    [Column("EmailID")]
    public long EmailId { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("EmailReceivers")]
    public virtual Email EmailNavigation { get; set; }
}
