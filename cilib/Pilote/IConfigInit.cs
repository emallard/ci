

using System.Net;

public interface IConfigInit
{

    string DomainName {get;}
    IPAddress PiloteIp { get; }
    string PiloteRepositoryPort { get; }


    bool CleanVaultImage { get; }
    bool CleanVaultContainer { get; }
    bool CreateVaultContainer { get; }

    bool CreateRootCA { get; }

    bool CleanRegistryImage { get; }
    bool CleanRegistryContainer { get; }
    bool CreateRegistryContainer {get;}
}