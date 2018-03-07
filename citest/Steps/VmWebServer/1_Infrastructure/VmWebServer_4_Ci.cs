using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmWebServer_4_Ci : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmWebServer vmWebServer;

        public VmWebServer_4_Ci(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmWebServer = infrastructure.GetVmWebServer();
        }

        public void Test()
        {
            // is image here
            var result = vmWebServer.SshCommand("docker images");
            Assert.Contains("ciexe", result);

            // try to run an image
            result = vmWebServer.SshCommand("docker run --rm --name ciexe ciexe hello");
            Assert.Contains("hello", result);
        }

        public void Run()
        {
            //vmWebServer.InstallCi();
        }

        public void Clean()
        {
            //vmWebServer.CleanCi();
        }
    }
}
