using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class VmWebServer_5_Traefik : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmWebServer vmWebServer;
        private readonly CiCli cli;

        public VmWebServer_5_Traefik(IInfrastructure infrastructure, CiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
            this.cli = cli.SetVm(vmWebServer);
        }

        public void Test()
        {
            var result = vmWebServer.SshCommand("curl http://localhost:80/");
            Assert.AreEqual("{}", result);
        }

        public void Run()
        {
            cli.InstallTraefik.SshCall();
        }

        public void Clean()
        {
            cli.CleanTraefik.SshCall();
        }
    }
}
