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
    public class PiloteInstallVault : IStep 
    {
        private readonly PiloteStep pstep;

        public PiloteInstallVault(
            PiloteStep pstep)
        {
            this.pstep = pstep;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            //var sshConnection = await pstep.GetPiloteSshConnection();
            //var vaultInstallation = cli.InstallVault.SshCall();
            await Task.CompletedTask;
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            var client = await pstep.GetPiloteSshClient2();
            var result = client.SshCommand("curl http://127.0.0.1:8200/v1/");
            StepAssert.Contains("{", result);
        }

    }
}