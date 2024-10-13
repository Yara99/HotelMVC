using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
