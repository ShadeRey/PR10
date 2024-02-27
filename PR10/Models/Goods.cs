using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace PR10.Models;

public class Goods
{
    public int Id { get; set; }
    public string VendorCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Unit { get; set; }
    public double? Price { get; set; } = null;
    public double? MaximumPossibleDiscountSize { get; set; } = null;
    public int Manufacturer { get; set; }
    public int Purveyor { get; set; }
    public int ProductCategory { get; set; }
    public double? CurrentDiscount { get; set; } = null;
    public int? QuantityInStock { get; set; } = null;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Manufacturer_name { get; set; } = string.Empty;

    public Uri ImageUrl => new Uri($"avares://PR10/Assets/GoodsPhoto/{Image}");

    public Bitmap Bitmap {
        get
        {
            using var stream = AssetLoader.Open(ImageUrl);
            return new Bitmap(stream);
        }
    }
}
