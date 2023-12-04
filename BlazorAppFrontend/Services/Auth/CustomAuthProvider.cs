using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAppTEST.Services.Auth;

public class CustomAuthProvider: AuthenticationStateProvider
{
    private readonly IUserLogin authService;

    public CustomAuthProvider(IUserLogin authService)
    {
        this.authService = authService;
        authService.OnAuthStateChanged += AuthStateChanged;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal principal = await authService.GetAuthAsync();
        
        return new AuthenticationState(principal);
    }
    
    private void AuthStateChanged(ClaimsPrincipal principal)
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(principal)
            )
        );
    }
}