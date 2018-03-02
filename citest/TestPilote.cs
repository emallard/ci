using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class TestPilote
    {
        private IContainer container;

        public TestPilote()
        {
            this.container = new TestInit().Init();
        }

        [Fact]
        public void InstallPiloteDockerRegistry()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.InstallMirrorRegistry();
        }


        // PiloteVm is already created
        // Install script : dotnetcore images, ciexe
        [Fact]
        public void InstallPilote()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.InstallDocker();

            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("docker run hello-world");
                Assert.Contains("Hello from Docker!", cmd.Result);
            }

            vmPilote.InstallCi();

            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("docker run ciexe hello");
                Assert.Contains("hello", cmd.Result);
            }
        }

    }
}
