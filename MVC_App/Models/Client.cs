using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class Client
{
    [Key]
    [Column("Client_ID")]
    public int ClientId { get; set; }

    [Column("AFM")]
    [StringLength(50)]
    [Display(Name = "AFM")]
    public string? Afm { get; set; }

    [Column("phoneNumber")]
    [StringLength(15)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Column("User_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Clients")]
    public virtual User User { get; set; } = null!;
}
