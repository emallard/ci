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
    public class WebServerInstallTraefikSsh : IStep
    {
        private readonly WebServerStep pstep;
        private readonly SshTraefik sshTraefik;

        public WebServerInstallTraefikSsh(
            WebServerStep pstep,
            SshTraefik sshTraefik)
        {
            this.pstep = pstep;
            this.sshTraefik = sshTraefik;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            sshTraefik.InstallTraefik(await pstep.GetWebServerSshConnection());
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            /*
            pstep.sshClient.Connect(await pstep.GetWebServerSshConnection());
            var result = pstep.sshClient.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            */
        }
    }
}