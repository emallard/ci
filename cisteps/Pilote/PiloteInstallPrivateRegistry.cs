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
    public class PiloteInstallPrivateRegistry : IStep 
    {
        private readonly AskHelper helper;
        private readonly ListResources listResources;
        private readonly IInfrastructure infrastructure;

        public PiloteInstallPrivateRegistry(
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

            infrastructure.GetVmPilote(sshConnection).BuildCiUsingSdk();
            
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            await Task.CompletedTask;
        }
/*
        // HOST
        public void Test()
        {
            var result = vmPilote.SshCommand($"getent hosts {vmPilote.PrivateRegistryDomain}");
            Assert.Contains(vmPilote.PrivateRegistryDomain, result);
        }

        public void Run()
        {
            vmPilote.InstallHosts();
        }

        public void Clean()
        {
            vmPilote.CleanHosts();
        }
         */
    }
}