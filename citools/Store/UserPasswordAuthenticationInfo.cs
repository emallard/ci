
namespace citools
{
    public class UserPasswordAuthenticationInfo : IAuthenticationInfo
    {
        public readonly string User;
        public readonly string Password;

        public UserPasswordAuthenticationInfo(string user, string password)
        {
            this.User = user;
            this.Password = password;
        }
    }
}