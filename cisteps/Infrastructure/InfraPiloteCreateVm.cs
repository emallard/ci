using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;

namespace cisteps
{
    public class InfraPiloteCreateVm : IStep
    {
        private readonly ListAsk listAsk;
        private readonly ListResources listResources;
        private readonly IInfrastructure infrastructure;
        private readonly ISshClient sshClient;

        public InfraPiloteCreateVm(
            ListAsk listAsk,
            ListResources listResources,
            IInfrastructure infrastructure,
            ISshClient sshClient)
        {
            this.listAsk = listAsk;
            this.infrastructure = infrastructure;
            this.listResources = listResources;
            this.sshClient = sshClient;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            //var devopInfraToken = await helper.Ask(DevopInfraToken);
            
            var apikey = await listAsk.InfraApiKey.Ask();
            var piloteRootPassword = await listAsk.PiloteRootPassword.Ask();
            var piloteUser = await listAsk.PiloteAdminUser.Ask();
            var pilotePassword = await listAsk.PiloteAdminPassword.Ask();
       
       
            
            infrastructure.CreateVm(apikey, "pilote", piloteRootPassword, piloteUser, pilotePassword);
            var uri = infrastructure.GetVmSshUri(apikey, "pilote");
            var piloteSshUri = uri.ToString();


            //IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(devopInfraToken);
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());
            

            await listResources.InfrastructureApiKey.Write(auth, apikey);
            await listResources.PiloteRootPassword.Write(auth, piloteRootPassword);
            await listResources.PiloteUser.Write(auth, piloteUser);
            await listResources.PilotePassword.Write(auth, pilotePassword);
            await listResources.PiloteSshUri.Write(auth, piloteSshUri);
        }

        public async Task Check()
        {
            var apikey = await listAsk.InfraApiKey.Ask();
            var piloteUser = await listAsk.PiloteAdminUser.Ask();
            var pilotePassword = await listAsk.PiloteAdminPassword.Ask();

            var client = sshClient.Connect(infrastructure.GetVmSshConnection(apikey, "pilote", piloteUser, pilotePassword));
            var result = client.Command("echo coucou");
            StepAssert.AreEqual("coucou\n", result);
            await Task.CompletedTask;
        }
    }
}