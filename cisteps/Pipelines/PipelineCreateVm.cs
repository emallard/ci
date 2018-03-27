using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineCreatePiloteVm : IPipeline
    {
        Func<IPipelineRunner, Task> run;

        PipelineCreatePiloteVm(
            InfraPiloteCreateVm infraPiloteCreateVm
            )
        {
            this.run = async (IPipelineRunner runner) => {
                await runner.Run(infraPiloteCreateVm);
            };
        }
        
        public async Task Run(IPipelineRunner runner)
        {
            await this.run(runner);
        }
    }
}