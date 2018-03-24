using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_3_InstallPrivateRegistry : IStep
    {
        private readonly CiCli cli;
        private readonly AskParameters askParameters;

        public PiloteCi_3_InstallPrivateRegistry(
            AskParameters askParameters, 
            CiCli cli)
        {
            this.cli = cli.SetSshConnection(askParameters.PiloteSshConnection());
            cli.SetVaultToken(askParameters.PiloteCiVaultToken);
            this.askParameters = askParameters;
        }

        public void Test()
        {

            var uri = askParameters.PrivateRegistryUri.ToString() + "/v2/";
            // First, test insecure, it doesn't check certificate
            {
                var result = cli.SshCommand($"curl --insecure {uri}");
                Assert.IsTrue(result.StartsWith("{"));
            }
            // Then test secure
            {
                var result = cli.SshCommand($"curl {uri}");
                Assert.IsTrue(result.StartsWith("{"));
            }
        }

        public void Run()
        {
            cli.InstallRegistry.SshCall();
        }

        public void Clean()
        {
            cli.CleanRegistry.SshCall();
        }
    }
}
