using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_Build : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        

        public PiloteCi_1_Build(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker images -a");
            Assert.Contains("dotnetcore_0", result);
        }

        public void Run()
        {
            vmPilote.Build();
        }

        public void Clean()
        {
        }
    }
}
