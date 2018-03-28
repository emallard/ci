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
    public class PiloteInstallMirrorRegistry : IStep
    {
        private readonly PiloteStep pstep;
        private readonly SshMirrorRegistry sshMirrorRegistry;

        public PiloteInstallMirrorRegistry(
            PiloteStep pstep,
            SshMirrorRegistry sshMirrorRegistry)
        {
            this.pstep = pstep;
            this.sshMirrorRegistry = sshMirrorRegistry;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            sshMirrorRegistry.InstallMirrorRegistry(await pstep.GetPiloteSshConnection());
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task CheckRunOk()
        {
            pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());
            var result = pstep.sshClient.Command("curl http://localhost:4999/v2/");
            StepAssert.AreEqual("{}", result);
        }
    }
}