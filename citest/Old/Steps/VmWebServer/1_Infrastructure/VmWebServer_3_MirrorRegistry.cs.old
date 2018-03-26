using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmWebServer_3_MirrorRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmWebServer vmWebServer;

        public VmWebServer_3_MirrorRegistry(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
        }

        public void Test()
        {
            var result = vmWebServer.SshCommand("curl http://localhost:4999/v2/");
            Assert.AreEqual("{}", result);
        }

        public void Run()
        {
            vmWebServer.InstallMirrorRegistry();
        }

        public void Clean()
        {
        }
    }
}
