using ClassLibrary_SEP3;
using System.Security.Claims;
using System.Text.Json;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services.Auth;

public class UserService : IUserLogin
{
    //HTTPClient
    private readonly HttpClient httpClient;
    public static string? Jwt { get; private set; } = "";

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;

    public UserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }


    public async Task createUser(User user)
    {
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
        {
            throw new Exception("One or many forms is empty, please fill them out before creating a new user");
        }

        if (user.Password.Length < 4)
        {
            throw new Exception("Password must be more then 4 characters");
        }

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Broker/User", user);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create user: {error}");
        }
    }


    public async Task Login(User user)
    {
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync("api/Broker/User/Login", user);

        //Guards
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
        {
            throw new Exception("One or many forms is empty, please fill them out before logging in");
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            var Token = await responseMessage.Content.ReadFromJsonAsync<TokenResponse>();
            Jwt = Token.Jwt;
            ClaimsPrincipal principal = CreateClaimsPrincipal();

            OnAuthStateChanged.Invoke(principal);
        }
        else
        {
            throw new Exception($"Failed to login: {responseMessage.StatusCode}");
        }
    }
    private static ClaimsPrincipal CreateClaimsPrincipal()
    {
        if (string.IsNullOrEmpty(Jwt))
        {
            return new ClaimsPrincipal();
        }

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);

        ClaimsIdentity identity = new(claims, "jwt");

        ClaimsPrincipal principal = new(identity);
        return principal;
    }

    public Task LogoutAsync()
    {
        Jwt = null;
        ClaimsPrincipal principal = new();
        OnAuthStateChanged.Invoke(principal);
        return Task.CompletedTask;
    }
    public Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        return Task.FromResult(principal);
    }
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }

}