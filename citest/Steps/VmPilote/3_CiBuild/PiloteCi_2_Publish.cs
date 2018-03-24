using System;
using Autofac;
using cicli;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_Publish : IStep
    {
        private readonly CiCli cli;

        public PiloteCi_2_Publish(AskParameters askParameters, CiCli cli)
        {
            this.cli = cli.SetSshConnection(askParameters.PiloteSshConnection());
            cli.SetVaultToken(askParameters.PiloteCiVaultToken);
        }

        public void Test()
        {
            var result = cli.SshCommand("curl -X GET https://privateregistry.mynetwork.local:5443/v2/_catalog");
            Assert.Contains("dotnetcore_0", result);
        }

        public void Run()
        {
            cli.PublishWebApp1.SshCall();
        }

        public void Clean()
        {
            cli.UnpublishWebApp1.SshCall();
        }
    }
}
