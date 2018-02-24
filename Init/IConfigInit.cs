

using System.Net;

public interface IConfigInit
{
    bool CleanVaultImage { get; }
    bool CleanVaultContainer { get; }
    bool CreateVaultContainer { get; }

    bool CreateRootCA { get; }

    bool CleanRegistryImage { get; }
    bool CleanRegistryContainer { get; }
    bool CreateRegistryContainer {get;}
}