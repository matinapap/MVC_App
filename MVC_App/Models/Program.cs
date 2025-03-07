using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public class Programm
{
    [Key]
    [StringLength(50)]
    public string ProgrammName { get; set; } = null!;

    public string? Benfits { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Charge { get; set; }

    [InverseProperty("ProgrammNameNavigation")]
    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
