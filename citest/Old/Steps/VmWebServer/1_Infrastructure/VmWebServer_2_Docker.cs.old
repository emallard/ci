using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmWebServer_2_Docker : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmWebServer vmWebServer;

        public VmWebServer_2_Docker(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
        }


        public void Test()
        {
            var result = vmWebServer.SshCommand("docker run --rm hello-world");
            Assert.Contains("Hello from Docker!", result);
        }

        public void Run()
        {
            vmWebServer.InstallDocker();
        }

        public void Clean()
        {
        }
    }
}
