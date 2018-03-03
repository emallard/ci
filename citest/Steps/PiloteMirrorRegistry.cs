using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class PiloteMirrorRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;

        public PiloteMirrorRegistry(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("curl http://localhost:4999/v2/");
                Assert.AreEqual("{}", cmd.Result);
            }
        }

        public void Run()
        {
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.InstallMirrorRegistry();
        }

        public void Revert()
        {
        }
    }
}
