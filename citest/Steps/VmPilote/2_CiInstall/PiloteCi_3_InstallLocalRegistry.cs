using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_3_InstallLocalRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteCi_3_InstallLocalRegistry(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Ssh())
            {
                //var cmd = client.RunCommand("docker exec ciexe dotnet ciexe.dll hello");
                //Assert.Contains("hello", cmd.Result);
            }
        }

        public void Run()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.InstallCi();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
