using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VGroupUser
{
    [StringLength(500)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool? Active { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    [Column("ID")]
    public long? Id { get; set; }

    public bool UserActive { get; set; }

    [StringLength(101)]
    public string UserName { get; set; }
}
