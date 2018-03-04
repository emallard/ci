using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_SetSourceInBuildContainer : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteCi_2_SetSourceInBuildContainer(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Ssh())
            {
                var cmd = client.RunCommand("docker exec ciexe_build ls /sources/");
                Assert.Contains("dotnet_core0", cmd.Result);
            }
        }

        public void Run()
        {
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.SetSourcesInBuildContainer();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
