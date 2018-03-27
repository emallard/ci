
namespace citools
{
    public class TokenAuthenticationInfo : IAuthenticationInfo
    {
        public readonly string Token;

        public TokenAuthenticationInfo(string token)
        {
            this.Token = token;
        }
    }
}