using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3;

public class User
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public List<String> ProjectID; 
}
