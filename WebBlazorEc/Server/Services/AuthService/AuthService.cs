namespace WebBlazorEc.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        public Task<ServiceResponse<int>> RegisterUser(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExits(string email)
        {
            throw new NotImplementedException();
        }
    }
}
