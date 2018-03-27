using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;

namespace cisteps
{
    public class PiloteInstallDocker : IStep
    {
        private readonly AskHelper helper;
        private readonly ListResources listResources;
        private readonly IInfrastructure infrastructure;

        public PiloteInstallDocker(
            AskHelper helper,
            ListResources listResources,
            IInfrastructure infrastructure)
        {
            this.listResources = listResources;
            this.infrastructure = infrastructure;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            var vaultUri = new Uri(await helper.Ask("vaultUri"));
            var vaultToken = await helper.Ask("vaultToken");
            IAuthenticationInfo auth = new TokenAuthenticationInfo(vaultToken);

            var sshConnection = await listResources.PiloteSshConnection.Read(vaultUri, auth);

            infrastructure.GetVmPilote(sshConnection).InstallDocker();
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }



        public async Task CheckRunOk()
        {
            var vaultUri = new Uri(await helper.Ask("vaultUri"));
            var vaultToken = await helper.Ask("vaultToken");
            IAuthenticationInfo auth = new TokenAuthenticationInfo(vaultToken);

            var sshConnection = await listResources.PiloteSshConnection.Read(vaultUri, auth);

            var client = new SshClient2().SetConnection(sshConnection);
            var result = client.SshCommand("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}