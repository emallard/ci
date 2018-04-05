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
    public class InstallTraefikSsh : IStep
    {
        private readonly CommonStep pstep;
        private readonly SshTraefik sshTraefik;
        private Func<Task<SshConnection>> getSshConnection;

        public InstallTraefikSsh(
            CommonStep pstep,
            SshTraefik sshTraefik)
        {
            this.pstep = pstep;
            this.sshTraefik = sshTraefik;
        }

        public void SetSshConnectionFunc(Func<Task<SshConnection>> getSshConnection)
        {
            this.getSshConnection = getSshConnection;
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            sshTraefik.InstallTraefik(await this.getSshConnection());
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