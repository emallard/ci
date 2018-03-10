using System;
using Autofac;
using Renci.SshNet;

namespace citest
{
    public class PiloteCi_2_Publish : IStep
    {
        private readonly IInfrastructure infrastructure;
        private readonly IVmPilote vmPilote;
        private readonly VmCiCli cli;

        public PiloteCi_2_Publish(IInfrastructure infrastructure, VmCiCli cli)
        {
            this.infrastructure = infrastructure;
            this.vmPilote = infrastructure.GetVmPilote();
            this.cli = cli.SetVm(vmPilote);
        }

        public void Test()
        {
            var result = vmPilote.SshCommand("curl -X GET https://privateregistry.mynetwork.local:5443/v2/_catalog");
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
