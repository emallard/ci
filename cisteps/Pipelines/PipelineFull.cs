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

        PipelineFull(
            IStepRunner runner,
            PipelineInit pipelineInit,
            PipelineCreatePiloteVm pipelineCreatePiloteVm,
            PipelineInstallPiloteVm pipelineInstallPiloteVm
            )
        {
            this.run = async () => {
                await pipelineInit.Run();
                await pipelineCreatePiloteVm.Run();
                await pipelineInstallPiloteVm.Run();
            };
        }
        
        public async Task Run()
        {
            await this.run();
        }
    }
}