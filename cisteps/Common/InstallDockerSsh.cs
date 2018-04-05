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
    public class InstallDockerSsh : IStep
    {
        private readonly CommonStep pstep;
        private readonly SshDocker sshDocker;
        private Func<Task<SshConnection>> getSshConnection;

        public InstallDockerSsh(
            CommonStep pstep,
            SshDocker sshDocker)
        {
            this.pstep = pstep;
            this.sshDocker = sshDocker;
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
            sshDocker.InstallDocker(await this.getSshConnection());
        }

        public async Task Check()
        {
            pstep.sshClient.Connect(await this.getSshConnection());
            var result = pstep.sshClient.Command("docker run --rm hello-world");
            StepAssert.Contains("Hello from Docker!", result);
            await Task.CompletedTask;
        }
    }
}