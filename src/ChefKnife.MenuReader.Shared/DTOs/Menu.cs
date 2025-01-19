using System;
using System.Collections.Generic;

namespace ChefKnife.MenuReader.Shared.DTOs;

public class Menu
{
    public IEnumerable<string> Titles { get; set; } = new List<string>();
    public IEnumerable<MenuItem> ItemsCol1 { get; set; } = new List<MenuItem>();
    public IEnumerable<MenuItem> ItemsCol2 { get; set; } = new List<MenuItem>();
    public IEnumerable<MenuItem> ItemsCol3 { get; set; } = new List<MenuItem>();
    public IEnumerable<MenuItem> ItemsCol4 { get; set; } = new List<MenuItem>();
}