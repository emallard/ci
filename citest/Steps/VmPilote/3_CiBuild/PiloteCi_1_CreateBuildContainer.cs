using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_CreateBuildContainer : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteCi_1_CreateBuildContainer(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Ssh())
            {
                var cmd = client.RunCommand("docker ps -a");
                Assert.Contains("ciexe_build", cmd.Result);
            }
        }

        public void Run()
        {
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.CreateBuildContainer();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
