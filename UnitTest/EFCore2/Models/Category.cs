using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore2.Models;

[Table("Category")]
public class Category
{
    public int CategoryId { get; set; }
    
    public string CategoryName { get; set; } = null!;

    public ICollection<Product> Products { get; } = new List<Product>();
}