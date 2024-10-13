using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Contactus
{
    public decimal Id { get; set; }

    public string? Contactaddress { get; set; }

    public string? Contactemail { get; set; }

    public string? Contactphone { get; set; }
}
