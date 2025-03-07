using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Models;

public partial class User
{
    [Key]
    [Column("User_id")]
    public int UserId { get; set; }

    [Column("First_Name")]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Column("Last_Name")]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? Username { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("User")]
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    [InverseProperty("User")]
    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();
}
