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
            PiloteInstallDotNetCoreSdk piloteInstallDotNetCoreSdk,
            PiloteCiexeSource piloteCiexeSource,
            PiloteCiexeBuildUsingSdk piloteCiexeBuildUsingSdk,
            PiloteInstallVault piloteInstallVault,
            PiloteInstallPrivateRegistry piloteInstallPrivateRegistry,
            PiloteInstallCA piloteInstallCA
            )
        {
            this.run = async () => {
                await runner.Run(piloteInstallDocker);
                await runner.Run(piloteInstallMirrorRegistry);
                await runner.Run(piloteInstallDotNetCoreSdk);
                await runner.Run(piloteCiexeSource);
                await runner.Run(piloteCiexeBuildUsingSdk);
                await runner.Run(piloteInstallVault);
                await runner.Run(piloteInstallPrivateRegistry);
                await runner.Run(piloteInstallCA);
            };
        }

        public async Task Run()
        {
            await this.run();
        }
    }
}