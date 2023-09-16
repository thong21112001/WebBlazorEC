using System.Security.Cryptography;

namespace WebBlazorEc.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<int>> RegisterUser(User user, string password)
        {
            if (await UserExits(user.Email))
            {
                return new ServiceResponse<int> 
                {
                    Success = false,
                    Message = "User already exits."
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
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

        //Tạo băm mật khẩu
        private void CreatePasswordHash(string passW, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //sử dụng thuật toán mã hoá mật khẩu
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passW));

            }
        }
    }
}
