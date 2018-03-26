using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using ciinfra;
using ciexecommands;

namespace cisteps
{
    public class PiloteStep
    {
        private readonly AskHelper helper;
        private readonly ListResources listResources;
        private readonly IInfrastructure infrastructure;
        private readonly CiExeCommands ciExeCommands;

        public PiloteStep(
            AskHelper helper,
            ListResources listResources,
            IInfrastructure infrastructure,
            CiExeCommands ciExeCommands)
        {
            this.listResources = listResources;
            this.infrastructure = infrastructure;
            this.ciExeCommands = ciExeCommands;
        }

        public async Task<IVmPilote> GetVmPilote()
        {
            var vmpilote = infrastructure.GetVmPilote(await GetPiloteSshConnection());
            return vmpilote;
        }

        public async Task<SshConnection> GetPiloteSshConnection()
        {
            var vaultUri = new Uri(await helper.Ask("vaultUri"));
            var vaultToken = await helper.Ask("vaultToken");
            IAuthenticationInfo auth = new TokenAuthenticationInfo(vaultToken);

            return await listResources.PiloteSshConnection.Read(vaultUri, auth);
        }

        public async Task<SshClient2> GetPiloteSshClient2()
        {
            var sshConnection = await GetPiloteSshConnection();
            var client = new SshClient2().SetConnection(sshConnection);
            return client;
        }

/*
        public async Task<CiExeCommands> GetPiloteCiexe()
        {
            var vaultUri = new Uri(await helper.Ask("ciExeVaultUri"));
            var vaultToken = await helper.Ask("ciExeVaultToken");

            ciExeCommands.Configure(
                askParameters.PiloteSshConnection(),
                askParameters.VaultUri,
                new VaultToken(""));

            var vmpilote = infrastructure.GetVmPilote(await GetPiloteSshConnection());
            return vmpilote;
        }
*/
    }
}