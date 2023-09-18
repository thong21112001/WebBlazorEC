namespace WebBlazorEc.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterUser(UserRegister userRequest);
        Task<ServiceResponse<string>> Login(UserLogin userRequest);
    }
}
