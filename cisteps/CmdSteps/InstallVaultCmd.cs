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
    public class InstallVaultCmd : IStep
    {
        private readonly SshStep pstep;
        private readonly CmdVault cmdVault;
        private ICommandExecute commandExecute;

        public InstallVaultCmd(
            SshStep pstep,
            CmdVault cmdVault)
        {
            this.pstep = pstep;
            this.cmdVault = cmdVault;
        }

        public void SetCommandExecute(ICommandExecute commandExecute)
        {
            this.commandExecute = commandExecute;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            await Task.CompletedTask;
            cmdVault.InstallVaultNoTls(commandExecute);
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            StepAssert.Contains("{}", commandExecute.Command("curl http://127.0.0.1:8200"));
        }
    }
}