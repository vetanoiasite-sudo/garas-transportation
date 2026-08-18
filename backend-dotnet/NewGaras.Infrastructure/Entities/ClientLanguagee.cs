using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ClientLanguagee
{
    [Key]
    public int Id { get; set; }

    public long ClientId { get; set; }

    public int LanguageeId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientLanguagees")]
    public virtual Client Client { get; set; }

    [ForeignKey("LanguageeId")]
    [InverseProperty("ClientLanguagees")]
    public virtual Languagee Languagee { get; set; }
}
