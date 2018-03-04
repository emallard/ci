using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_InstallCA : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteCi_1_InstallCA(IInfrastructure infrastructure)
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
            var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.InstallCi();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
