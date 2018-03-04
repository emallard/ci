using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_SetSourceInBuildContainer : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        

        public PiloteCi_2_SetSourceInBuildContainer(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker exec ciexe_build ls /sources/");
            Assert.Contains("dotnet_core0", result);
        }

        public void Run()
        {
            vmPilote.SetSourcesInBuildContainer();
        }

        public void Clean()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
