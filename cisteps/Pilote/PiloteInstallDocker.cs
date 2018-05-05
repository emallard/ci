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
        private readonly CmdDocker cmdDocker;

        public PiloteInstallDocker(
            PiloteStep pstep,
            CmdDocker cmdDocker)
        {
            this.pstep = pstep;
            this.cmdDocker = cmdDocker;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            await Task.CompletedTask;
            //sshDocker.InstallDocker(await pstep.GetPiloteSshConnection());
        }

        public async Task Check()
        {
            pstep.sshClient.Connect(await pstep.GetPiloteSshConnection());
            var result = pstep.sshClient.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}