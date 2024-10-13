using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMVC.Models;

public partial class Hotel
{
    public decimal Hotelid { get; set; }

    public string? Hotelname { get; set; }

    public string? Hoteladdress { get; set; }

    public string? Hotelphone { get; set; }

    public string? Hotelemail { get; set; }

    public string? Hoteldescription { get; set; }

    public string? Hotelimage { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
