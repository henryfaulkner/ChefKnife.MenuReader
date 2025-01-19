using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.Shared.DTOs;

public class MenuItem
{
    public string? Name { get; set; }
    public double? Price { get; set; } // Nullable in case the price is missing
    public string? Ingredients { get; set; }
}
