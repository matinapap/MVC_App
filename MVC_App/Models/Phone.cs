using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class Phone
{
    [Key]
    [StringLength(15)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(50)]
    public string ProgrammName { get; set; } = null!;

    [InverseProperty("PhoneNumberNavigation")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [ForeignKey("ProgrammName")]
    [InverseProperty("Phones")]
    public virtual Programm ProgrammNameNavigation { get; set; } = null!;
}
