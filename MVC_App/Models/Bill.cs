using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class Bill
{
    [Key]
    [Column("Bill_ID")]
    public int BillId { get; set; }

    [StringLength(15)]
    public string PhoneNumber { get; set; } = null!;

    [Column(TypeName = "decimal(7, 2)")]
    public decimal? Costs { get; set; }

    [ForeignKey("PhoneNumber")]
    [InverseProperty("Bills")]
    [Display(Name = "Phone Number")]
    public virtual Phone PhoneNumberNavigation { get; set; } = null!;

    [ForeignKey("BillId")]
    [InverseProperty("Bills")]
    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
}
