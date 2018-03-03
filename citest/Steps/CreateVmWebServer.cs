using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class CreateVmWebServer : IStep {

    private readonly IInfrastructure infrastructure;

    public CreateVmWebServer(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
    }

    public void Test()
    {
        var vmWebServer = infrastructure.GetVmWebServer();
        using (var sshClient = vmWebServer.Connect())
        {
            var cmd = sshClient.RunCommand("echo coucou");
            Assert.IsTrue("coucou\n" == cmd.Result);
        }
    }

    public void Run()
    {
        infrastructure.CreateVmWebServer();
    }

    public void Revert()
    {
        infrastructure.DeleteVmWebServer();
    }
}