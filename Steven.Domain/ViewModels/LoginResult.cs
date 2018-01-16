using Steven.Domain.Enums;
using Steven.Domain.Models;

namespace Steven.Domain.ViewModels
{
    public class LoginResult
    {
        public SigninStatus Status { get; set; }
        public Users UserInfo { get; set; }

    }
}
