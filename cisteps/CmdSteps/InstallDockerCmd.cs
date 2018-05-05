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
    public class InstallDockerCmd : IStep
    {
        private readonly SshStep pstep;
        private readonly CmdDocker cmdDocker;
        private ICommandExecute commandExecute;

        public InstallDockerCmd(
            SshStep pstep,
            CmdDocker sshDocker)
        {
            this.pstep = pstep;
            this.cmdDocker = sshDocker;
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
            await Task.CompletedTask;
            cmdDocker.InstallDocker(commandExecute);
        }

        public async Task Check()
        {
            var result = commandExecute.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}