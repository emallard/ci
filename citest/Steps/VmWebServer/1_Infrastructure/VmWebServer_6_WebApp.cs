using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmWebServer_6_WebApp : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmWebServer vmWebServer;
        private readonly VmCiCli cli;

        public VmWebServer_6_WebApp(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
            this.cli = new VmCiCli().SetVm(vmWebServer);
        }

        public void Test()
        {
            //var result = vmWebServer.SshCommand("curl http://localhost:4999/v2/");
            //Assert.AreEqual("{}", result);
        }

        public void Run()
        {
            //cli.InstallWebApp();
        }

        public void Clean()
        {
        }
    }
}
