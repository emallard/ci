using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_5b_PiloteCi_BuildUsingSdk : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_5b_PiloteCi_BuildUsingSdk(IInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
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
            try
            {
                vmPilote.SshCommand("dotnet --version");
            }
            catch(Exception)
            {
                vmPilote.InstallDotNetCoreSdk();
            }
            
            vmPilote.CloneOrPullCiSources();
            vmPilote.BuildCiUsingSdk();
        }

        public void Clean()
        {
            vmPilote.CleanCiImage();
        }
    }
}
