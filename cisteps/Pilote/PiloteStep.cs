using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using ciexecommands;

namespace cisteps
{
    public class PiloteStep
    {
        public readonly ListAsk listAsk;
        public readonly ListResources listResources;
        public readonly CiExeCommands ciExeCommands;
        public readonly ISshClient sshClient;

        public PiloteStep(
            ListAsk listAsk,
            ListResources listResources,
            CiExeCommands ciExeCommands,
            ISshClient sshClient)
        {
            this.listResources = listResources;
            this.ciExeCommands = ciExeCommands;
            this.sshClient = sshClient;
            this.listAsk = listAsk;
        }

        public async Task<SshConnection> GetPiloteSshConnection()
        {
            //IAuthenticationInfo auth = new TokenAuthenticationInfo(await listAsk.LocalVaultToken.Ask());

            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());
            return await listResources.PiloteSshConnection.Read(auth);
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