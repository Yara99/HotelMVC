using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMVC.Models;

public partial class Room
{
    public decimal Roomid { get; set; }

    public decimal? Roomprice { get; set; }

    public decimal? Roomcapacity { get; set; }

    public string? Roomdescription { get; set; }

    public string? Availabilitystatus { get; set; }

    public string? Roomimage { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public decimal? Hotelid { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
