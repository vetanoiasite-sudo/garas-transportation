using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientNATIONAL")]
public partial class ClientNational
{
    [Key]
    public int Id { get; set; }

    [Column("Client_Id")]
    public long ClientId { get; set; }

    [Column("National_Id")]
    public int NationalId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientNationals")]
    public virtual Client Client { get; set; }

    [ForeignKey("NationalId")]
    [InverseProperty("ClientNationals")]
    public virtual National National { get; set; }
}
