using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ContractType")]
public partial class ContractType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public string Description { get; set; }

    [InverseProperty("ContactType")]
    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();
}
