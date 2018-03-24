using System;
using Autofac;
using cicli;
using ciinfra;
using cilib;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_InstallVault : IStep
    {
        private readonly IKeepResult keepResult;
        private readonly CiCli cli;

        public PiloteCi_2_InstallVault(
            AskParameters askParameters,
            IKeepResult keepResult,
            CiCli cli)
        {
            this.keepResult = keepResult;
            this.cli = cli.SetSshConnection(askParameters.PiloteSshConnection());
            this.cli.SetVaultToken(new VaultToken(""));
        }

        public void Test()
        {
            //var result = vmPilote.SshCommand("docker exec ciexe dotnet ciexe.dll hello");
            //Assert.Contains("hello", cmd.Result);
        }

        public void Run()
        {
            var vaultInstallation = cli.InstallVault.SshCall();
            keepResult.Keep("vaultInstallation", vaultInstallation);
        }

        public void Clean()
        {
        }
    }
}
