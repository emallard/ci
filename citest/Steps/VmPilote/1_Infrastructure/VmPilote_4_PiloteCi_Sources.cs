using System;
using Autofac;
using ciinfra;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_4_PiloteCi_Sources : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_4_PiloteCi_Sources(
            IInfrastructure infrastructure,
            AskParameters askParameters)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote(askParameters.PiloteSshConnection());
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("cd ~/ci && git config --get remote.origin.url");
            Assert.Contains("https://github.com/emallard/ci.git", result);
        }

        public void Run()
        {
            vmPilote.CloneOrPullCiSources();
        }

        public void Clean()
        {
            vmPilote.CleanCiSources();
        }
    }
}
