namespace WebBlazorEc.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterUser(User user, string password);
        Task<bool> UserExits(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
    }
}
