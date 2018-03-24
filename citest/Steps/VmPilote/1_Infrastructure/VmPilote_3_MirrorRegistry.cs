using System;
using Autofac;
using ciinfra;
using Renci.SshNet;

namespace citest
{
    public class VmPilote_3_MirrorRegistry : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;

        public VmPilote_3_MirrorRegistry(
            IInfrastructure infrastructure,
            AskParameters askParameters)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote(askParameters.PiloteSshConnection());
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("curl http://localhost:4999/v2/");
            Assert.AreEqual("{}", result);
        }

        public void Run()
        {
            vmPilote.InstallMirrorRegistry();
        }

        public void Clean()
        {
        }
    }
}
