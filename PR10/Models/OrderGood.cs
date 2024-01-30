using System;

namespace PR10.Models;

public class OrderGood
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Good { get; set; } = String.Empty;
    public int Quantity { get; set; }
}