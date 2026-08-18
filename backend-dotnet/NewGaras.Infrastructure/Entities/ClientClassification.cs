using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientClassification")]
public partial class ClientClassification
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [InverseProperty("ClientClassification")]
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
