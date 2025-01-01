using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using DemoApp.Models;

namespace DemoApp.Infrastructure
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var sessionResult = await _protectedLocalStorage.GetAsync<SessionModel>("demo-session");
                if (sessionResult.Success)
                {
                    var session = sessionResult.Value;

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "User"),
                    new Claim(ClaimTypes.NameIdentifier, session.Sesssionid.ToString())
                };

                    var identity = new ClaimsIdentity(claims, "Custom Authentication");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
            }
            catch { }

            var anonymousIdentity = new ClaimsIdentity();
            var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
            return new AuthenticationState(anonymousUser);
        }

        public void MarkUserAsAuthenticated(SessionModel session)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "User"),
                new Claim(ClaimTypes.NameIdentifier, session.Sesssionid.ToString())
            };
            var identity = new ClaimsIdentity(claims, "Custom Authentication");
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
            var authState = new AuthenticationState(anonymousUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
    }
}
