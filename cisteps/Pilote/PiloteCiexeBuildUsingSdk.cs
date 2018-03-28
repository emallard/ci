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
    public class PiloteCiexeBuildUsingSdk : IStep 
    {
         private readonly PiloteStep pstep;
        private readonly SshCiexe sshCiexe;

        public PiloteCiexeBuildUsingSdk(
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
            var connection = await pstep.GetPiloteSshConnection();
            pstep.sshClient.Connect(connection);

            try
            {
                pstep.sshClient.Command("dotnet --version");
            }
            catch(Exception)
            {
                sshCiexe.InstallDotNetCoreSdk(connection);
            }
            
            sshCiexe.CloneOrPullCiSources(connection);
            sshCiexe.BuildCiUsingSdk(connection);
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            var client = pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());

            // is image here
            var result = client.Command("docker images");
            StepAssert.Contains("ciexe", result);

            // try to run an image
            result = client.Command("docker run --rm --name ciexe ciexe hello");
            StepAssert.Contains("hello", result);

            await Task.CompletedTask;
        }

    }
}