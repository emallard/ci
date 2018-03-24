using System;
using Autofac;
using cicli;
using ciinfra;
using cilib;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_1_InstallCA : IStep
    {
        private readonly CiCli cli;

        public PiloteCi_1_InstallCA(
            AskParameters askParameters, 
            CiCli cli)
        {
            this.cli = cli.Configure(
                askParameters.PiloteSshConnection(),
                new Uri("a"),
                new VaultToken(""));
        }

        public void Test()
        {
            var infraCidata = "/cidata";
            var result = cli.SshCommand("cat " + infraCidata + "/tls/myCA.pem");
            Assert.IsTrue(result.Length > 3);
        }

        public void Run()
        {
            cli.InstallCA.SshCall();
        }

        public void Clean()
        {
            cli.CleanCA.SshCall();
        }
    }
}
