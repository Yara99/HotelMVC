using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMVC.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string? Userfname { get; set; }

    public string? Userlname { get; set; }

    public string? Userphone { get; set; }

    public string? Useremail { get; set; }

    public string? Imagepath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
