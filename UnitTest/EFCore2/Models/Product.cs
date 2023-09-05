using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore2.Models;

[Table("Product")]
public class Product
{
    //[Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
    public string? Manufacture { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}