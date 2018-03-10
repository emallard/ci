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

        public VmWebServer_6_WebApp(IInfrastructure infrastructure, VmCiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
            this.cli = cli.SetVm(vmWebServer);
        }

        public void Test()
        {
            var result = vmWebServer.SshCommand("curl http://webapp.mynetwork.local/");
            Assert.AreEqual("OK", result);
        }

        public void Run()
        {
            cli.InstallWebApp1.SshCall();
        }

        public void Clean()
        {
            cli.CleanInstallWebApp1.SshCall();
        }
    }
}
