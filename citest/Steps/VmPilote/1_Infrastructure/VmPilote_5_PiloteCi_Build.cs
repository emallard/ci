using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_5_PiloteCi_Build : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_5_PiloteCi_Build(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("docker images");
            Assert.Contains("ciexe", result);
        }

        public void Run()
        {
            vmPilote.BuildCiImage();
        }

        public void Clean()
        {
            vmPilote.CleanCiImage();
        }
    }
}
