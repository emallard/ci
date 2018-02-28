using System;
using Autofac;
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
                Assert.Equal("coucou", cmd.Result);
            }
        }

        // PiloteVm is already created
        // Install script : docker, dotnetcore images, ciexe
        //[Fact]
        public void TestInstallPilote()
        {
            var infrastructure = this.container.Resolve<VBoxInfrastructure>();
            var vmPilote = infrastructure.GetVmPilote();
            vmPilote.Install();

            using (var client = vmPilote.Connect())
            {
                var cmd = client.RunCommand("docker run hello-world");
                Assert.Contains("ok", cmd.Result);

                cmd = client.RunCommand("docker run ciexe hello");
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
    }
}
