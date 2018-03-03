using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class PiloteDocker : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteDocker(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Connect())
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
