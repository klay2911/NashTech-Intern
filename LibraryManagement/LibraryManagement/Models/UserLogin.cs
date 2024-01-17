using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class UserLogin
{
    public string? OldPassword { get; set; }
    [Required]
    [DisplayName("Username")]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}