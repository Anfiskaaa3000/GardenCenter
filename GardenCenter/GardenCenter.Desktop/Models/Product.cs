using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public decimal BasePrice { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    // Путь к картинке для отображения в интерфейсе
    [NotMapped]
    public Avalonia.Media.Imaging.Bitmap? FullImagePath { get; set; }
}