using System;
using Autofac;
using ciinfra;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_5b_PiloteCi_BuildUsingSdk : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_5b_PiloteCi_BuildUsingSdk(
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
            vmPilote.CloneOrPullCiSources();
            vmPilote.BuildCiUsingSdk();
        }

        public void Clean()
        {
            vmPilote.CleanCiImage();
        }
    }
}
