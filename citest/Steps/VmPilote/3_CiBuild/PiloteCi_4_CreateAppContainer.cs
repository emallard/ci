using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_4_CreateAppContainer : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        

        public PiloteCi_4_CreateAppContainer(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker exec ciexe dotnet ciexe.dll hello");
            Assert.Contains("hello", result);
        }

        public void Run()
        {
            vmPilote.CreateAppContainer();
        }

        public void Clean()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
