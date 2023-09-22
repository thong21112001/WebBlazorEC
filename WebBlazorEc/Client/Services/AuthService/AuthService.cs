using Microsoft.AspNetCore.Components.Authorization;

namespace WebBlazorEc.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient http,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword userRequest)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", userRequest.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        //Xem user da dang nhap hay chua
        public async Task<bool> IsUserAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<ServiceResponse<string>> Login(UserLogin userRequest)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", userRequest);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> RegisterUser(UserRegister userRequest)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", userRequest);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
