using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace ciinfra
{
    public class InfraPiloteInstallDocker : IStep
    {
        private readonly StepHelper helper;
        private readonly IInfrastructure infrastructure;

        public InfraPiloteInstallDocker(
            StepHelper helper,
            IInfrastructure infrastructure)
        {
            this.helper = helper.SetStep(this);
            this.infrastructure = infrastructure;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            var vaultUri = await helper.AskVaultUriAndToken();
            var sshConnection = await helper.NeedSshConnection(vaultUri, "pilote");
            infrastructure.GetVmPilote(sshConnection).InstallDocker();
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task TestRunOk()
        {
            var vaultUri = await helper.AskVaultUriAndToken();
            var sshConnection = await helper.NeedSshConnection(vaultUri, "pilote");

            var client = new SshClient2().SetConnection(sshConnection);
            var result = client.SshCommand("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}