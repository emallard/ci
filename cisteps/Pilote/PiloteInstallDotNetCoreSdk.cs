using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;

namespace cisteps
{
    public class PiloteInstallDotNetCoreSdk : IStep
    {
        private readonly PiloteStep pstep;

        public PiloteInstallDotNetCoreSdk(
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
            vmPilote.InstallDotNetCoreSdk();
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task TestRunOk()
        {
            var client = await pstep.GetPiloteSshClient2();
            var result = client.SshCommand("dotnet --version");
            StepAssert.StartsWith(result, "2");
        }
    }
}