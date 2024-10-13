using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMVC.Models;

public partial class Home
{
    public decimal Id { get; set; }

    public string? Logo { get; set; }

    public string? Hometitle { get; set; }

    public string? Homecontent { get; set; }

    public string? Homeimage { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

}
