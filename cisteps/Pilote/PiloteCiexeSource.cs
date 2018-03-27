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
    public class PiloteCiexeSource : IStep 
    {
        private readonly PiloteStep pstep;

        public PiloteCiexeSource(
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
            var vmPilote = await pstep.GetVmPilote();
            vmPilote.CloneOrPullCiSources();
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            var client = await pstep.GetPiloteSshClient2();

            var result = client.SshCommand("cd ~/ci && git config --get remote.origin.url");
            StepAssert.Contains("https://github.com/emallard/ci.git", result);
        }

    }
}