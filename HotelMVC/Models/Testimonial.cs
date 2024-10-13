using System;
using System.Collections.Generic;

namespace HotelMVC.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string? Testimonialcontent { get; set; }

    public DateTime? Testimonialdate { get; set; }

    public string? Testimonialstatus { get; set; }

    public decimal? Userid { get; set; }

    public virtual User? User { get; set; }
}
