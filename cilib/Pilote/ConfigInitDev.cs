

using System.Net;
using System.Threading.Tasks;

public class ConfigVBox : IConfig
{
    public IPAddress PiloteIp => new IPAddress(new byte[]{10,0,2,4});
    public string DomainName => "mynetwork.local";
    public string PiloteRepositoryPort => "5443";
  
}


