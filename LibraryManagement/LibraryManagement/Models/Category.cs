using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models;

[Table("Category")]
public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Book> Books { get; } = new List<Book>();
  
}