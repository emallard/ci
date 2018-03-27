using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class PipelineRunnerRunAll : IPipelineRunner
    {
        public async Task Run(IStep step)
        {
            try {
                await step.CheckRunOk();
                return;
            }
            catch (Exception) {}

            try { 
                await step.Run();
            }
            catch (Exception) {}
        }
    }
}