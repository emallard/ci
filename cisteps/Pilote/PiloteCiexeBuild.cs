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
    public class PiloteCiexeBuild : IStep 
    {
        private readonly PiloteStep pstep;
        private readonly SshCiexe sshCiexe;

        public PiloteCiexeBuild(
            PiloteStep pstep,
            SshCiexe sshCiexe)
        {
            this.pstep = pstep;
            this.sshCiexe = sshCiexe;
        }

        public async Task Clean()
        {
            sshCiexe.CleanCiImage(await pstep.GetPiloteSshConnection());
        }

        public async Task Run()
        {
            sshCiexe.BuildCiImage(await pstep.GetPiloteSshConnection());
        }


        public async Task Check()
        {
            var client = pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());

            // is image here
            var result = client.Command("docker images");
            StepAssert.Contains("ciexe", result);

            // try to run an image
            result = client.Command("docker run --rm --name ciexe ciexe hello");
            StepAssert.Contains("hello", result);
        }

    }
}