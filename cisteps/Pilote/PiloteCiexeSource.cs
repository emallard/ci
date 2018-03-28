using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using ciinfra;
using cilib;

namespace cisteps
{
    public class PiloteCiexeSource : IStep 
    {
        private readonly PiloteStep pstep;
        private readonly SshCiexe sshCiexe;

        public PiloteCiexeSource(
            PiloteStep pstep,
            SshCiexe sshCiexe)
        {
            this.pstep = pstep;
            this.sshCiexe = sshCiexe;
        }

        public async Task Clean()
        {
            sshCiexe.CleanCiSources(await pstep.GetPiloteSshConnection());
        }

        public async Task Run()
        {
            sshCiexe.CloneOrPullCiSources(await pstep.GetPiloteSshConnection());
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            var client = pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());

            var result = client.Command("cd ~/ci && git config --get remote.origin.url");
            StepAssert.Contains("https://github.com/emallard/ci.git", result);
        }

    }
}