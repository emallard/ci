using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_InstallCA : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public PiloteCi_1_InstallCA(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("cat ~/ci-data/myCA.key");
            Assert.IsTrue(result.Length > 3);
        }

        public void Run()
        {
            vmPilote.InstallCA();
        }

        public void Clean()
        {
            vmPilote.CleanCA();
        }
    }
}
