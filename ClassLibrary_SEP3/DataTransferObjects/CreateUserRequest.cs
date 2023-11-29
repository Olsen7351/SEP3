using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class CreateUserRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}