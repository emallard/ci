using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_3_MirrorRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;

        public VmPilote_3_MirrorRegistry(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public void Test()
        {
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Ssh())
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
