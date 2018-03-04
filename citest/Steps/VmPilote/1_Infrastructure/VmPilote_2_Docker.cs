using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_2_Docker : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_2_Docker(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }


        public void Test()
        {
            var result = vmPilote.SshCommand("docker run --rm hello-world");
            Assert.Contains("Hello from Docker!", result);
        }

        public void Run()
        {
            vmPilote.InstallDocker();
        }

        public void Clean()
        {
        }
    }
}
