using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Userlogin
{
    public decimal Id { get; set; }

    public string? Username { get; set; }

    public string? Userpassword { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roleid { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
