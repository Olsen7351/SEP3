using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class CreateUserRequest
{
    [Required]
    public string username { get; init; }
    [Required]
    public string password { get; set; }
}