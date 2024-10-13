using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Bankaccount
{
    public decimal Id { get; set; }

    public decimal? Cardnumber { get; set; }

    public decimal? Cvv { get; set; }

    public DateTime? Expirydate { get; set; }

    public decimal? Balance { get; set; }
}
