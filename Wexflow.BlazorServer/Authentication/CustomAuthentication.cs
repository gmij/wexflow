using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Wexflow.BlazorServer.Authentication
{
    public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        private IServiceScopeFactory _scopeFactory;
        private readonly ILocalStorageService storageService;
        private readonly IdentityOptions _options;

        public CustomAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILocalStorageService storageService)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            this.storageService = storageService;
            _options = optionsAccessor.Value;
        }

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var hasLogin = await storageService.ContainKeyAsync("token");

            return hasLogin ? MockState : MockAnonymous;
            //return Task.FromResult(MockAnonymous);
            //var identity = new ClaimsIdentity(ClaimTypes.Name, "admin", "测试用户");
            //return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        private AuthenticationState MockState => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "admin") }, "apiauth_ype")));

        private AuthenticationState MockAnonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public async void Login()
        {
            await storageService.SetItemAsync("token", MockState.User.FindFirstValue(ClaimTypes.Name));
            NotifyAuthenticationStateChanged(Task.FromResult(MockState));
        }

        public async void Logout()
        {
            await storageService.ClearAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(MockAnonymous));
        }
    }
}
