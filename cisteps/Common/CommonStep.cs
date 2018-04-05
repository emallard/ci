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
    public class CommonStep
    {
        public readonly ListAsk listAsk;
        public readonly ListResources listResources;
        public readonly CiExeCommands ciExeCommands;
        public readonly ISshClient sshClient;

        public CommonStep(
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
/*
        public async Task<SshConnection> GetWebServerSshConnection()
        {
            //IAuthenticationInfo auth = new TokenAuthenticationInfo(await listAsk.LocalVaultToken.Ask());

            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());
            return await listResources.WebServerSshConnection.Read(auth);
        }
*/
    }
}