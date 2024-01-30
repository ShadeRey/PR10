using System;

namespace PR10.Models;

public class Goods
{
    public string VendorCode { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public int Unit { get; set; }
    public double Price { get; set; }
    public double MaximumPossibleDiscountSize { get; set; }
    public int Manufacturer { get; set; }
    public int Purveyor { get; set; }
    public int ProductCategory { get; set; }
    public double CurrentDiscount { get; set; }
    public int QuantityInStock { get; set; }
    public string Description { get; set; } = String.Empty;
    public string Image { get; set; } = String.Empty;
}