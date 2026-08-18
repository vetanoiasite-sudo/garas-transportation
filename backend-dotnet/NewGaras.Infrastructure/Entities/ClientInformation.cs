using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ClientInformation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? Number { get; set; }

    public string Image { get; set; }

    public long ClientId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientInformations")]
    public virtual Client Client { get; set; }
}
