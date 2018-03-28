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
        Func<Task> run;

        public PipelineCreatePiloteVm(
            InfraPiloteCreateVm infraPiloteCreateVm,
            IStepRunner runner
            )
        {
            this.run = async () => {
                await runner.Run(infraPiloteCreateVm);
            };
        }
        
        public async Task Run()
        {
            await this.run();
        }
    }
}