using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citest;
using ciinfra;

namespace citest
{
    public class VmPilote_1_Create : IStep {

        private readonly IInfrastructure infrastructure;
        private readonly AskParameters askParameters;
        private readonly IKeepResult keepResult;
        private readonly IVmPilote vmPilote;

        public VmPilote_1_Create(
            IInfrastructure infrastructure,
            AskParameters askParameters,
            IKeepResult keepResult)
        {
            this.infrastructure = infrastructure;
            this.askParameters = askParameters;
            this.keepResult = keepResult;
        }

        public void Test()
        {
            using (var client = infrastructure.Ssh(
                askParameters.InfrastructureKey, 
                askParameters.PiloteVmName, 
                askParameters.PiloteAdminUser, 
                askParameters.PiloteAdminPassword))
            {
                var result = client.RunCommand("echo coucou");
                Assert.IsTrue("coucou\n" == result.Result);
            }
            
        }

        public void Run()
        {
            infrastructure.CreateVm(
                askParameters.InfrastructureKey, 
                askParameters.PiloteRootPassword, 
                askParameters.PiloteVmName, 
                askParameters.PiloteAdminUser, 
                askParameters.PiloteAdminPassword);

            keepResult.Keep("PiloteSshUri", infrastructure.GetVmSshUri(askParameters.InfrastructureKey, askParameters.PiloteVmName).ToString());
        }

        public void Clean()
        {
            infrastructure.DeleteVm(
                askParameters.InfrastructureKey, 
                askParameters.PiloteVmName);
        }
    }
}