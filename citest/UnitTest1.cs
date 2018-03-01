using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class UnitTest1
    {
        private IContainer container;

        public UnitTest1()
        {
            this.container = new TestInit().Init();
        }

        [Fact]
        public void CreateVmPilote()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            infrastructure.DeleteVmPilote();
            infrastructure.CreateVmPilote();
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("echo coucou");
                Assert.Equal("coucou\n", cmd.Result);
            }
        }

        // PiloteVm is already created
        // Install script : docker, dotnetcore images, ciexe
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

        // Pilote is installed with docker, dotnetcore images (and registry, vault ?)
        [Fact]
        public void TestRun()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmPilote = infrastructure.GetVmPilote();
        }

        [Fact]
        public void ConnectToClonedVm()
        {
            var connectionInfo = new ConnectionInfo(
            "127.0.0.1", 22200, "test",
            new PasswordAuthenticationMethod("test", "test"));
            using (var sshClient = new SshClient(connectionInfo))
            {
                sshClient.Connect();
                sshClient.RunCommand("echo coucou");
            }
        }
    }
}
