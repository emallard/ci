using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;

namespace ciinfra
{
    public class InfraPiloteCreateVm : IStep
    {
        public const string VaultUri = "vault uri";
        public const string DevopInfraToken = "devop-infra token";

        public const string ApiKey = "apikey";
        public const string RootPassword = "root password";
        public const string AdminUser = "admin user";
        public const string AdminPassword = "admin password";

        private Uri vaultUri;
        private string devopInfraToken;

        private string apikey;
        private string rootPassword;
        private string adminUser;
        private string adminPassword;
        
        private string piloteSshUri;

        private readonly IAsk ask;
        private readonly IInfrastructure infrastructure;

        public InfraPiloteCreateVm(
            IAsk ask,
            IInfrastructure infrastructure)
        {
            this.ask = ask;
            this.infrastructure = infrastructure;
        }

        public async Task Ask()
        {
            this.vaultUri = new Uri(await this.ask.GetValue(VaultUri));
            this.devopInfraToken = await this.ask.GetValue(DevopInfraToken);
            
            this.apikey = await ask.GetValue(ApiKey);
            this.rootPassword = await ask.GetValue(RootPassword);
            this.adminUser = await ask.GetValue(AdminUser);
            this.adminPassword = await ask.GetValue(AdminPassword);
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Keep()
        {
            // log with token
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(devopInfraToken);
            var client = VaultSharp.VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);
            
            await client.WriteSecretAsync("/infra/apikey", new Dictionary<string, object>
                    {{ "value", this.apikey }});

            await client.WriteSecretAsync("/infra/rootPassword", new Dictionary<string, object>
                    {{ "value", this.rootPassword }});

            await client.WriteSecretAsync("/admin/pilote/user", new Dictionary<string, object>
                    {{ "value", this.adminUser }});

            await client.WriteSecretAsync("/admin/pilote/password", new Dictionary<string, object>
                    {{ "value", this.adminPassword }});

            await client.WriteSecretAsync("/admin/pilote/sshuri", new Dictionary<string, object>
                    {{ "value", this.piloteSshUri }});

        }

        public Task Need()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            infrastructure.CreateVm(new InfrastructureKey(apikey), "pilote", this.rootPassword, this.adminUser, this.adminPassword);
            var uri = infrastructure.GetVmSshUri(new InfrastructureKey(apikey), "pilote");
            this.piloteSshUri = uri.ToString();
            await Task.CompletedTask;
        }

        public async Task TestAlreadyRun()
        {
            infrastructure.VmExists(new InfrastructureKey(apikey), "pilote");
            await Task.CompletedTask;
        }

        public async Task TestRunOk()
        {
            var client = infrastructure.Ssh(new InfrastructureKey(apikey), "pilote", this.adminUser, this.adminPassword);
            var result = client.RunCommand("echo coucou");
            StepAssert.AreEqual("coucou\n", result.Result);
            await Task.CompletedTask;
        }
    }
}