using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMVC.Models;

public partial class About
{
    public decimal Id { get; set; }

    public string? Abouttitle { get; set; }

    public string? Aboutcontent { get; set; }

    public string? Aboutimage { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
}
