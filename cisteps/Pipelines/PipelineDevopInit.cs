using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{
    public class PipelineDevopInit : IPipeline
    {
        Func<Task> run;

        public PipelineDevopInit(
            IStepRunner runner,
            DevOpInitVault devopInitVault,
            DevOpInitSsl devopInitSsl )
        {
            this.run = async () => {
                await runner.Run(devopInitVault);
                await runner.Run(devopInitSsl);
            };
        }

        public async Task Run()
        {
            await this.run();
        }
    }

    
}