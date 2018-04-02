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
    public class WebServerInstallDocker : IStep
    {
        private readonly WebServerStep pstep;
        private readonly SshDocker sshDocker;

        public WebServerInstallDocker(
            WebServerStep pstep,
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
            sshDocker.InstallDocker(await pstep.GetWebServerSshConnection());
        }

        public async Task Check()
        {
            pstep.sshClient.Connect(await pstep.GetWebServerSshConnection());
            var result = pstep.sshClient.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}