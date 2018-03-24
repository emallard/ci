using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_Build : IStep
    {
        private readonly CiCli cli;

        public PiloteCi_1_Build(AskParameters askParameters, CiCli cli)
        {
            this.cli = cli.Configure(
                askParameters.PiloteSshConnection(),
                askParameters.VaultUri,
                askParameters.PiloteCiVaultToken);
        }

        public void Test()
        {
            var result = cli.SshCommand("docker images -a");
            Assert.Contains("dotnetcore_0", result);
        }

        public void Run()
        {
            cli.BuildWebApp1.SshCall();
        }

        public void Clean()
        {
            cli.CleanWebApp1.SshCall();
        }
    }
}
