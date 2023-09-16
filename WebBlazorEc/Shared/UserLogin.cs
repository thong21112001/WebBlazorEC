using System.ComponentModel.DataAnnotations;

namespace WebBlazorEc.Shared
{
    internal class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
