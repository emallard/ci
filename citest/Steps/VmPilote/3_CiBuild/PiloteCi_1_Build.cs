using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_Build : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly CiCli cli;

        public PiloteCi_1_Build(IInfrastructure infrastructure, CiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = cli.SetVm(vmPilote);
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker images -a");
            Assert.Contains("dotnetcore_0", result);
        }

        public void Run()
        {
            cli.BuildWebApp1.SshCall();
        }

        public void Clean()
        {
            cli.CleanWebApp1.SshCall();
        }
    }
}
