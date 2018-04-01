using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineFull : IPipeline
    {
        Func<Task> run;

        public PipelineFull(
            IStepRunner runner,
            PipelineDevopInit pipelineInit,
            PipelineCreatePiloteVm pipelineCreatePiloteVm,
            PipelineInstallPiloteVm pipelineInstallPiloteVm
            )
        {
            this.run = async () => {
                await pipelineInit.Run();
                await pipelineCreatePiloteVm.Run();
                await pipelineInstallPiloteVm.Run();
                //await pipelineCreateWebServerVm.Run();
                //await pipelineInstallWebServerVm.Run();
            };
        }
        
        public async Task Run()
        {
            await this.run();
        }
    }
}