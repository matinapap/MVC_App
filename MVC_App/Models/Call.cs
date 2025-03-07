using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class Call
{
    [Key]
    [Column("Call_ID")]
    public int CallId { get; set; }

    public string? Description { get; set; }

    [ForeignKey("CallId")]
    [InverseProperty("Calls")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
