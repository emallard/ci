using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using cilib;

namespace cisteps
{
    public class AddGitToBuild : IStep
    {
        private readonly SshStep pstep;
        private readonly IGit git;

        public AddGitToBuild(
            SshStep pstep,
            IGit git)
        {
            this.pstep = pstep;
            this.git = git;
        }

        public async Task Clean()
        {
            var auth = await GetAuthentication();

            await pstep.listResources.GitUri.DeleteIfExists(auth);
            await pstep.listResources.GitDirectory.DeleteIfExists(auth);
        }

        public async Task Run()
        {
            var auth = await GetAuthentication();

            var gitUri = await pstep.listAsk.GitUri.Ask();
            var gitDirectory = await pstep.listAsk.GitDirectory.Ask();

            await pstep.listResources.GitUri.Write(auth, gitUri);
            await pstep.listResources.GitDirectory.Write(auth, gitUri);
        }

        public async Task Check()
        {
            var auth = await GetAuthentication();

            await pstep.listResources.GitUri.Read(auth);
            await pstep.listResources.GitDirectory.Read(auth);
        }

        private async Task<IAuthenticationInfo> GetAuthentication()
        {
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await pstep.listAsk.LocalVaultDevopUser.Ask(),
                await pstep.listAsk.LocalVaultDevopPassword.Ask());
            return auth;
        }
    }
}