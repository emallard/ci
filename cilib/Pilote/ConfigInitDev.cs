

using System.Net;
using System.Threading.Tasks;

public class ConfigInitDev : IConfigInit
{
    
    public bool CleanVaultImage => false;
    public bool CleanVaultContainer => true;
    public bool CreateVaultContainer => true;

    public bool CreateRootCA => true;

    public bool CleanRegistryImage => false;
    public bool CleanRegistryContainer => true;
    public bool CreateRegistryContainer => true;
    
}


