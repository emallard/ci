

using System.Net;
using System.Threading.Tasks;

public class ConfigInitDev : IConfigInit
{
    public IPAddress PiloteIp => new IPAddress(new byte[]{10,0,2,4});
    public string DomainName => "mynetwork.local";
    public string PiloteRepositoryPort => "5443";

    public bool CleanVaultImage => false;
    public bool CleanVaultContainer => true;
    public bool CreateVaultContainer => true;

    public bool CreateRootCA => true;

    public bool CleanRegistryImage => false;
    public bool CleanRegistryContainer => true;
    public bool CreateRegistryContainer => true;
    
}


