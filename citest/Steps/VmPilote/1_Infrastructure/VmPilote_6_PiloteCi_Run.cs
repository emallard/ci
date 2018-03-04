using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_6_PiloteCi_Run : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_6_PiloteCi_Run(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker start ciexe && docker exec ciexe dotnet ciexe.dll hello");
            Assert.Contains("hello", result);
        }

        public void Run()
        {
            vmPilote.RunCiContainer();
        }

        public void Clean()
        {
            vmPilote.CleanCiContainer();
        }
    }
}
