using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using cilib;

namespace cisteps
{
    public class PiloteInstallDotNetCoreSdk : IStep
    {
        private readonly PiloteStep pstep;
        private readonly SshCiexe sshCiexe;

        public PiloteInstallDotNetCoreSdk(
            PiloteStep pstep,
            SshCiexe sshCiexe)
        {
            this.pstep = pstep;
            this.sshCiexe = sshCiexe;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            sshCiexe.InstallDotNetCoreSdk(await pstep.GetPiloteSshConnection());
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            var client = pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());
            var result = client.Command("dotnet --version");
            StepAssert.StartsWith(result, "2");
        }
    }
}