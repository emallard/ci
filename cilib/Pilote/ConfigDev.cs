

using System.Net;
using System.Threading.Tasks;

public class ConfigDev : IConfig 
{
    public string DomainName => "mynetwork.local";
    public string PiloteDomainName => "pilote.mynetwork.local";
    
    private IPAddress piloteIp = null;
    public IPAddress PiloteIp {
        get {
            if (piloteIp == null)
            {
                var host = Task.Run(() => Dns.GetHostEntryAsync(this.PiloteDomainName)).Result;
                piloteIp = host.AddressList[0];
            }
            return piloteIp;
        }
    }
    public string PiloteRepositoryPort => "5443";
}


