using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_2_Docker : IStep
    {
        private readonly IInfrastructure infrastructure;

        public VmPilote_2_Docker(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Ssh())
            {
                var cmd = client.RunCommand("docker run --rm hello-world");
                Assert.Contains("Hello from Docker!", cmd.Result);
            }
        }

        public void Run()
        {
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.InstallDocker();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
