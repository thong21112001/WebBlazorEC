namespace WebBlazorEc.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterUser(UserRegister userRequest);
    }
}
