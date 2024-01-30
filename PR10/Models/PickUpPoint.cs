using System;

namespace PR10.Models;

public class PickUpPoint
{
    public int Id { get; set; }
    public int Index { get; set; }
    public int City { get; set; }
    public string Address { get; set; } = String.Empty;
}