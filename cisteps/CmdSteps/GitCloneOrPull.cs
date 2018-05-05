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
    public class GitCloneOrPull : IStep
    {
        private readonly SshStep pstep;
        private readonly CmdGit cmdGit;
        private ICommandExecute commandExecute;

        public GitCloneOrPull(
            SshStep pstep,
            CmdGit cmdGit)
        {
            this.pstep = pstep;
            this.cmdGit = cmdGit;
        }

        public void SetCommandExecute(ICommandExecute commandExecute)
        {
            this.commandExecute = commandExecute;
        }

        public async Task Clean()
        {
            await Task.CompletedTask;
        }

        public async Task Run()
        {
            var uri = await pstep.listResources.GitUri.Read(await GetAuthentication());
            var dir = await pstep.listResources.GitDirectory.Read(await GetAuthentication());
            await cmdGit.CloneOrPull(commandExecute, new Uri(uri), dir);
        }

        public async Task Check()
        {
            await Task.CompletedTask;
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