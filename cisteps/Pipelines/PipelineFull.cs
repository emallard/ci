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
        Func<IPipelineRunner, Task> run;

        PipelineFull(
            PipelineInit pipelineInit,
            PipelineCreatePiloteVm pipelineCreatePiloteVm,
            PipelineInstallPiloteVm pipelineInstallPiloteVm
            )
        {
            this.run = async (IPipelineRunner runner) => {
                await pipelineInit.Run(runner);
                await pipelineCreatePiloteVm.Run(runner);
                await pipelineInstallPiloteVm.Run(runner);
            };
        }
        
        public async Task Run(IPipelineRunner runner)
        {
            await this.run(runner);
        }
    }
}