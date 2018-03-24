using System;
using Autofac;
using ciinfra;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_5_PiloteCi_Build : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_5_PiloteCi_Build(
            IInfrastructure infrastructure,
            AskParameters askParameters)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote(askParameters.PiloteSshConnection());
        }

        public void Test()
        {
            // is image here
            var result = vmPilote.SshCommand("docker images");
            Assert.Contains("ciexe", result);

            // try to run an image
            result = vmPilote.SshCommand("docker run --rm --name ciexe ciexe hello");
            Assert.Contains("hello", result);
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
