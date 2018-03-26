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
    public class PiloteCiexeBuild : IStep 
    {
        private readonly PiloteStep pstep;

        public PiloteCiexeBuild(
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
            vmPilote.BuildCiImage();
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task TestRunOk()
        {
            var client = await pstep.GetPiloteSshClient2();

            // is image here
            var result = client.SshCommand("docker images");
            StepAssert.Contains("ciexe", result);

            // try to run an image
            result = client.SshCommand("docker run --rm --name ciexe ciexe hello");
            StepAssert.Contains("hello", result);
        }

    }
}