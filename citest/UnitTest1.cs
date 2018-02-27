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
            // VBoxClone.sh
        }

        [Fact]
        public void InstallPilote()
        {
            VBoxHelper.CheckVmStarted("pilote");
            var install = this.container.Resolve<InstallPilote>();
            //install.Install();
            install.CheckInstall();
        }        
    }
}
