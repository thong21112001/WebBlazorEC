namespace WebBlazorEc.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<int>> RegisterUser(UserRegister userRequest)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", userRequest);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
