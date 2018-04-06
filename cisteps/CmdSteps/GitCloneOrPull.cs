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
    public class GitCloneOrPull : IStep
    {
        private readonly SshStep pstep;
        private readonly IGit git;
        private Func<Task<SshConnection>> getSshConnection;

        public GitCloneOrPull(
            SshStep pstep,
            IGit git)
        {
            this.pstep = pstep;
            this.git = git;
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
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await pstep.listAsk.LocalVaultDevopUser.Ask(),
                await pstep.listAsk.LocalVaultDevopPassword.Ask());

            var gitUri = new Uri(await pstep.listResources.GitUri.Read(auth));
            var gitDirectory = await pstep.listResources.GitDirectory.Read(auth);

            pstep.sshClient.Connect(await this.getSshConnection());
            pstep.sshClient.Command(CloneOrPull(gitUri, gitDirectory));
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

        public string CloneOrPull(Uri gitUri, string directory)
        {
            var script = 
              "if [ -d \"" + directory + "\" ]; then" + "\n"
              + Pull(directory) + "\n"
              + "else" + "\n"
              + Clone(gitUri, directory) + "\n"
              + "fi" + "\n";

            return script;
        }

        public string Pull(string directory)
        {
            var cmd = $"git --git-dir=\"{directory}\" pull";
            return cmd;
        }

        public string Clone(Uri gitUri, string directory)
        {
            var cmd = "git clone " + "\"" + gitUri.ToString() + "\" \"" + directory + "\"";
            return cmd;
        }
    }
}