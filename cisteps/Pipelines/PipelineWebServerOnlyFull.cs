using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineWebServerOnlyFull : IPipeline
    {
        Func<Task> run;

        public PipelineWebServerOnlyFull(
            IStepRunner runner,
            PipelineDevopInit pipelineDevOpInit,
            InfraWebServerCreateVm infraWebServerCreateVm,
            WebServerInstallDocker webServerInstallDocker,
            WebServerInstallTraefikSsh webServerInstallTraefikSsh
            )
        {
            this.run = async () => {
                await pipelineDevOpInit.Run();
                await runner.Run(infraWebServerCreateVm);
                await runner.Run(webServerInstallDocker);
                await runner.Run(webServerInstallTraefikSsh);
            };
        }
        
        public async Task Run()
        {
            await this.run();
        }
    }
}