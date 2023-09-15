using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class UserLogin
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string Role { get; set; }
    public bool RememberMe { get; set; }
}