using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClassLibrary_SEP3;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ProjectMicroservice.Data;

namespace ProjectMicroservice.Services;

public class UserService: IUserService
{

    private readonly IMongoCollection<User> _users;
    private readonly string _jwtSecret;
    
    
    public UserService(MongoDbContext context)
    {
        _users = context.Database.GetCollection<User>("Users");
    }
    
    public User CreateUser(User user)
    {
        _users.InsertOne(user);
        return user;
    }
    
    
    public string Login(User user)
    {
        // Step 1: Validate the credentials
        var existingUser = _users.Find(u => u.Username == user.Username).FirstOrDefault();
        if (existingUser == null || !VerifyPassword(user.Password, existingUser.Password))
        {
            throw new Exception("Invalid credentials");
        }

        // Step 2: Generate Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username)
            // Add more claims as needed
        };

        // Step 3: Create the JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    
    
    private bool VerifyPassword(string providedPassword, string storedPassword)
    {
        return providedPassword == storedPassword;
    }
}