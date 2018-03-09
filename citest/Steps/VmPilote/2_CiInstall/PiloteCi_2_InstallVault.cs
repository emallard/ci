using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_InstallVault : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_2_InstallVault(IInfrastructure infrastructure, VmCiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = cli.SetVm(vmPilote);
        }

        public void Test()
        {
            //var result = vmPilote.SshCommand("docker exec ciexe dotnet ciexe.dll hello");
            //Assert.Contains("hello", cmd.Result);
        }

        public void Run()
        {
            cli.InstallVault.SshCall();
        }

        public void Clean()
        {
        }
    }
}
