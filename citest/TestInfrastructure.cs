using System;
using Autofac;
using Renci.SshNet;
using Xunit;

namespace citest
{
    public class TestInfrastructure
    {
        private IContainer container;

        public TestInfrastructure()
        {
            this.container = new TestInit().Init();
        }

        [Fact]
        public void CreateVmPilote()
        {
            var infrastructure = (IInfrastructure) this.container.Resolve<VBoxInfrastructure>();
            infrastructure.DeleteVmPilote();
            infrastructure.CreateVmPilote();
            var vmPilote = infrastructure.GetVmPilote();
            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("echo coucou");
                Assert.Equal("coucou\n", cmd.Result);
            }
        }

        [Fact]
        public void CreateVmWebServer()
        {
            var infrastructure = (IInfrastructure) this.container.Resolve<VBoxInfrastructure>();
            infrastructure.DeleteVmWebServer();
            infrastructure.CreateVmWebServer();
            var vmWebServer = infrastructure.GetVmWebServer();
            using (var client = vmWebServer.Connect())
            {
                var cmd = client.RunCommand("echo coucou");
                Assert.Equal("coucou\n", cmd.Result);
            }
        }

        [Fact]
        public void ConnectVmWebServer()
        {
            var infrastructure = (IInfrastructure) this.container.Resolve<VBoxInfrastructure>();
            var vmWebServer = infrastructure.GetVmWebServer();
            using (var client = vmWebServer.Connect())
            {
                var cmd = client.RunCommand("echo coucou");
                Assert.Equal("coucou\n", cmd.Result);
            }
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
