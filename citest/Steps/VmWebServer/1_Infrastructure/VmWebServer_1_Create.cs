using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmWebServer_1_Create : IStep {

    private readonly IInfrastructure infrastructure;
    private readonly IVmWebServer vmWebServer;

    public VmWebServer_1_Create(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
        this.vmWebServer = infrastructure.GetVmWebServer();
    }

    public void Test()
    {
        var result = vmWebServer.SshCommand("echo coucou");
        Assert.IsTrue("coucou\n" == result);
    }

    public void Run()
    {
        infrastructure.CreateVmWebServer();
    }

    public void Clean()
    {
        infrastructure.DeleteVmWebServer();
    }
}