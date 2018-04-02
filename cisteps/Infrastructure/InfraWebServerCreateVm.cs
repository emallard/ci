using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;

namespace cisteps
{
    public class InfraWebServerCreateVm : IStep
    {
        private readonly ListAsk listAsk;
        private readonly ListResources listResources;
        private readonly IInfrastructure infrastructure;
        private readonly ISshClient sshClient;

        public InfraWebServerCreateVm(
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
            var webServerRootPassword = await listAsk.WebServerRootPassword.Ask();
            var webServerUser = await listAsk.WebServerAdminUser.Ask();
            var webServerPassword = await listAsk.WebServerAdminPassword.Ask();
       
       
            
            infrastructure.CreateVm(apikey, "webServer", webServerRootPassword, webServerUser, webServerPassword);
            var uri = infrastructure.GetVmSshUri(apikey, "webServer");
            var webServerSshUri = uri.ToString();


            //IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(devopInfraToken);
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());
            

            await listResources.InfrastructureApiKey.Write(auth, apikey);
            await listResources.WebServerRootPassword.Write(auth, webServerRootPassword);
            await listResources.WebServerUser.Write(auth, webServerUser);
            await listResources.WebServerPassword.Write(auth, webServerPassword);
            await listResources.WebServerSshUri.Write(auth, webServerSshUri);
        }

        public async Task Check()
        {
            var apikey = await listAsk.InfraApiKey.Ask();
            var webServerUser = await listAsk.WebServerAdminUser.Ask();
            var webServerPassword = await listAsk.WebServerAdminPassword.Ask();

            var client = sshClient.Connect(infrastructure.GetVmSshConnection(apikey, "webServer", webServerUser, webServerPassword));
            var result = client.Command("echo coucou");
            StepAssert.AreEqual("coucou\n", result);
            await Task.CompletedTask;
        }
    }
}