using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class PiloteCi : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteCi(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("docker run --rm ciexe hello");
                Assert.Contains("hello", cmd.Result);
            }
        }

        public void Run()
        {
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.InstallCi();
        }

        public void Revert()
        {
            //var vmPilote = infrastructure.GetVmPilote();
            //vmPilote.CleanCi();
        }
    }
}
