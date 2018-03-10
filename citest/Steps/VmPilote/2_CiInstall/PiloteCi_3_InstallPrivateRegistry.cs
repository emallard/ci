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

        public PiloteCi_3_InstallPrivateRegistry(IInfrastructure infrastructure, VmCiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = cli.SetVm(vmPilote);
        }

        public void Test()
        {
            var domain = vmPilote.PrivateRegistryDomain;
            var port = vmPilote.PrivateRegistryPort;
            // First, test insecure
            {
                var result = vmPilote.SshCommand($"curl --insecure https://{domain}:{port}/v2/");
                Assert.IsTrue(result.StartsWith("{"));
            }
            // Then test secure
            {
                var result = vmPilote.SshCommand($"curl https://{domain}:{port}/v2/");
                Assert.IsTrue(result.StartsWith("{"));
            }
        }

        public void Run()
        {
            cli.InstallRegistry.SshCall();
        }

        public void Clean()
        {
            cli.CleanRegistry.SshCall();
        }
    }
}
