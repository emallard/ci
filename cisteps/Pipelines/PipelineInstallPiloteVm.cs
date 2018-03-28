using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineInstallPiloteVm : IPipeline
    {
        Func<Task> run;

        public PipelineInstallPiloteVm(
            IStepRunner runner,
            PiloteInstallDocker piloteInstallDocker,
            PiloteInstallMirrorRegistry piloteInstallMirrorRegistry,
            PiloteInstallPrivateRegistry piloteInstallPrivateRegistry,
            PiloteInstallDotNetCoreSdk piloteInstallDotNetCoreSdk
            )
        {
            this.run = async () => {
                await runner.Run(piloteInstallDocker);
                await runner.Run(piloteInstallMirrorRegistry);
                await runner.Run(piloteInstallPrivateRegistry);
                await runner.Run(piloteInstallDotNetCoreSdk);
            };
        }

        public async Task Run()
        {
            await this.run();
        }
    }
}