namespace WebBlazorEc.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public Task<ServiceResponse<int>> RegisterUser(User user, string password)
        {
            throw new NotImplementedException();
        }

        //Kiểm tra xem người dùng nào có email này không
        public async Task<bool> UserExits(string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }
    }
}
