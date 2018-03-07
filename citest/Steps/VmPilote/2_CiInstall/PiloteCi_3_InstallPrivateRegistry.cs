using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_3_InstallPrivateRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_3_InstallPrivateRegistry(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = new VmCiCli().SetVm(vmPilote);
        }

        public void Test()
        {
            var domain = vmPilote.PrivateRegistryDomain;
            var port = vmPilote.PrivateRegistryPort;
            var result = vmPilote.SshCommand($"curl https://{domain}:{port}/v2/");
            Assert.AreEqual("{}", result);
        }

        public void Run()
        {
            cli.InstallPrivateRegistry();
        }

        public void Clean()
        {
            cli.CleanPrivateRegistry();
        }
    }
}
