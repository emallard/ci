using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_Publish : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_2_Publish(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = new VmCiCli().SetVm(vmPilote);
        }

        public void Test()
        {
            //var result = vmPilote.SshCommand("docker exec ciexe dotnet ciexe.dll hello");
            //Assert.Contains("hello", result);
        }

        public void Run()
        {
            cli.PublishWebApp();
        }

        public void Clean()
        {
            //vmPilote.CleanCi();
        }
    }
}
