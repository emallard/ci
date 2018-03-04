using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_CreateBuildContainer : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        

        public PiloteCi_1_CreateBuildContainer(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker ps -a");
            Assert.Contains("ciexe_build", result);
        }

        public void Run()
        {
            vmPilote.CreateBuildContainer();
        }

        public void Clean()
        {
        }
    }
}
