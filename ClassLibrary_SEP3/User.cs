using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3;

public class User
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(16, ErrorMessage = "Username is too long, only 16 characters are allowed")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
