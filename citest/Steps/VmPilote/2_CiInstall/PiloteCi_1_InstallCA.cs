using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_InstallCA : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly CiCli cli;

        public PiloteCi_1_InstallCA(IInfrastructure infrastructure, CiCli cli)
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
