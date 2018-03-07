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

        public PiloteCi_1_InstallCA(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = new VmCiCli().SetVm(vmPilote);
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("cat ~/cidata/tls/myCA.pem");
            Assert.IsTrue(result.Length > 3);
        }

        public void Run()
        {
            cli.InstallCA();
        }

        public void Clean()
        {
            cli.CleanCA();
        }
    }
}
