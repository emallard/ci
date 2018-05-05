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
    public class DockerBuildCmd : IStep
    {
        private readonly SshStep pstep;
        private readonly CmdDocker cmdDocker;
        private ICommandExecute commandExecute;

        public DockerBuildCmd(
            SshStep pstep,
            CmdDocker cmdDocker)
        {
            this.pstep = pstep;
            this.cmdDocker = cmdDocker;
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
            cmdDocker.Build(commandExecute, await pstep.listResources.GitDirectory.Read(await GetAuthentication()));
            // Add tag
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            //cmdDocker.ImageExists(name,tag)
            StepAssert.IsTrue(false);
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