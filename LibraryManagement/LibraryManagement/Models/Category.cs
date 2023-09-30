using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models;

[Table("Category")]
public class Category
{    
    [DisplayName("Category Id")]
    public int CategoryId { get; set; }
    [DisplayName("Category Name")]
    public string CategoryName { get; set; }
    public ICollection<Book> Books { get; } = new List<Book>();

}