using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_Build : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_1_Build(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = new VmCiCli().SetVm(vmPilote);
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker images -a");
            Assert.Contains("dotnetcore_0", result);
        }

        public void Run()
        {
            cli.BuildWebApp();
        }

        public void Clean()
        {
        }
    }
}
