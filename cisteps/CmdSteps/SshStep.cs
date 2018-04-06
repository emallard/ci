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
    public class SshStep
    {
        public readonly ListAsk listAsk;
        public readonly ListResources listResources;
        public readonly ISshClient sshClient;

        public SshStep(
            ListAsk listAsk,
            ListResources listResources,
            ISshClient sshClient)
        {
            this.listResources = listResources;
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