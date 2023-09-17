using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models;

[Table("User")]
public class User
{
    public int UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }
    public int Age => DateTime.Now.Year - Dob.Year;
    public ICollection<BookBorrowingRequest> BookBorrowingRequests { get; } = new List<BookBorrowingRequest>();
}

