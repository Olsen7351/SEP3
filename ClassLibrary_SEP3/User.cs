using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3;

public class User
{
    [StringLength(16, ErrorMessage = "Username is to long, only 16 characters is allowed")]
    public String Username;
    public String Password;
}