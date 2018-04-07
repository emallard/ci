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
        private readonly IVaultSealKeys vaultSealKeys;
        private ICommandExecute commandExecute;

        public InstallVaultCmd(
            SshStep pstep,
            CmdVault cmdVault,
            IVaultSealKeys vaultSealKeys)
        {
            this.pstep = pstep;
            this.cmdVault = cmdVault;
            this.vaultSealKeys = vaultSealKeys;
        }

        public void SetCommandExecute(ICommandExecute commandExecute)
        {
            this.commandExecute = commandExecute;
        }

        public async Task Clean()
        {
            await Task.CompletedTask;
            cmdVault.CleanVaultNoTls(commandExecute);
        }

        public async Task Run()
        {
            await Task.CompletedTask;
            cmdVault.InstallVaultNoTls(commandExecute, vaultSealKeys);
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            cmdVault.Check(commandExecute);
            
        }
    }
}