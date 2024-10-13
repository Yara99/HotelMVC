using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Reservation
{
    public decimal Reservationid { get; set; }

    public DateTime? Checkindate { get; set; }

    public DateTime? Checkoutdate { get; set; }

    public decimal? Totalprice { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roomid { get; set; }

    public virtual Room? Room { get; set; }

    public virtual User? User { get; set; }
}
