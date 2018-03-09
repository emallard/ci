using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_InstallCA : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_1_InstallCA(IInfrastructure infrastructure, VmCiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = cli.SetVm(vmPilote);
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("cat ~/cidata/tls/myCA.pem");
            Assert.IsTrue(result.Length > 3);
        }

        public void Run()
        {
            cli.InstallCA.SshCall();
        }

        public void Clean()
        {
            cli.CleanCA.SshCall();
        }
    }
}
