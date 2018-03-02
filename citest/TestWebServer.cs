using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class TestWebServer
    {
        private IContainer container;

        public TestWebServer()
        {
            this.container = new TestInit().Init();
        }

        [Fact]
        public void InstallWebServerDocker()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmWebServer = infrastructure.GetVmWebServer();
            
            vmWebServer.InstallDocker();

            using (var client = vmWebServer.Connect())
            {
                var cmd = client.RunCommand("docker run hello-world");
                Assert.Contains("Hello from Docker!", cmd.Result);
            }
        }

        [Fact]
        public void InstallWebServerDockerRegistry()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmWebServer = infrastructure.GetVmWebServer();
            vmWebServer.InstallMirrorRegistry();
        }

        [Fact]
        public void InstallWebServerCi()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmWebServer = infrastructure.GetVmWebServer();

            vmWebServer.InstallCi();

            using (var client = vmWebServer.Connect())
            {
                var cmd = client.RunCommand("docker run ciexe hello");
                Assert.Contains("hello", cmd.Result);
            }
        }
    }
}
