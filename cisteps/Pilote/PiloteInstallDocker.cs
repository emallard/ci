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
    public class PiloteInstallDocker : IStep
    {
        private readonly PiloteStep pstep;
        private readonly SshDocker sshDocker;

        public PiloteInstallDocker(
            PiloteStep pstep,
            SshDocker sshDocker)
        {
            this.pstep = pstep;
            this.sshDocker = sshDocker;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            sshDocker.InstallDocker(await pstep.GetPiloteSshConnection());
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }

        public async Task CheckRunOk()
        {
            pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());
            var result = pstep.sshClient.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}