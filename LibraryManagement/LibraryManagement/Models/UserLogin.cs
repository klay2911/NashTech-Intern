using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class UserLogin
{
    public string? OldPassword { get; set; }
    [Required]
    [DisplayName("Username")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2}.", MinimumLength = 6)]  
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}