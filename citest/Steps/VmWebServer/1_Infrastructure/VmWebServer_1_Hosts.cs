using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmWebServer_1_Hosts : IStep {

    private readonly IInfrastructure infrastructure;
    private readonly IVmWebServer vmWebServer;

    public VmWebServer_1_Hosts(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
        this.vmWebServer = infrastructure.GetVmWebServer();
    }

    public void Test()
    {
        //var result = vmWebServer.SshCommand($"getent hosts {vmWebServer.PrivateRegistryDomain}");
        //Assert.Contains(vmWebServer.PrivateRegistryDomain, result);
    }

    public void Run()
    {
        //vmWebServer.InstallHosts();
    }

    public void Clean()
    {
        //vmWebServer.CleanHosts();
    }
}