using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using ciinfra;

namespace cisteps
{
    public class InfraPiloteCreateVm : IStep
    {
        public const string VaultUri = "vault uri";
        public const string DevopInfraToken = "devop-infra token";

        public const string ApiKey = "apikey";
        public const string RootPassword = "root password";
        public const string AdminUser = "admin user";
        public const string AdminPassword = "admin password";


        private readonly AskHelper helper;
        private readonly IInfrastructure infrastructure;
        private readonly ListResources listResources;

        public InfraPiloteCreateVm(
            AskHelper helper,
            IInfrastructure infrastructure,
            ListResources listResources)
        {
            this.helper = helper;
            this.infrastructure = infrastructure;
            this.listResources = listResources;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            var vaultUri = new Uri(await helper.Ask(VaultUri));
            var devopInfraToken = await helper.Ask(DevopInfraToken);
            
            var apikey = await helper.Ask(ApiKey);
            var rootPassword = await helper.Ask(RootPassword);
            var piloteUser = await helper.Ask(AdminUser);
            var pilotePassword = await helper.Ask(AdminPassword);
       
            
            infrastructure.CreateVm(new InfrastructureKey(apikey), "pilote", rootPassword, piloteUser, pilotePassword);
            var uri = infrastructure.GetVmSshUri(new InfrastructureKey(apikey), "pilote");
            var piloteSshUri = uri.ToString();


            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(devopInfraToken);
            await listResources.InfrastructureApiKey.Write(vaultUri, tokenAuthenticationInfo, apikey);
            await listResources.PiloteRootPassword.Write(vaultUri, tokenAuthenticationInfo, rootPassword);
            await listResources.PiloteUser.Write(vaultUri, tokenAuthenticationInfo, piloteUser);
            await listResources.PilotePassword.Write(vaultUri, tokenAuthenticationInfo, pilotePassword);
            await listResources.PiloteSshUri.Write(vaultUri, tokenAuthenticationInfo, piloteSshUri);
        }

        public async Task TestAlreadyRun()
        {
            var apikey = await helper.Ask(ApiKey);
            infrastructure.VmExists(new InfrastructureKey(apikey), "pilote");
            await Task.CompletedTask;
        }

        public async Task TestRunOk()
        {
            var apikey = await helper.Ask(ApiKey);
            var piloteUser = await helper.Ask(AdminUser);
            var pilotePassword = await helper.Ask(AdminPassword);

            var client = infrastructure.Ssh(new InfrastructureKey(apikey), "pilote", piloteUser, pilotePassword);
            var result = client.RunCommand("echo coucou");
            StepAssert.AreEqual("coucou\n", result.Result);
            await Task.CompletedTask;
        }
    }
}