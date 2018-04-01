using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{
    public class PipelineInit : IPipeline
    {
        private readonly DevOpInit ciInit;

        public PipelineInit(DevOpInit ciInit)
        {
            this.ciInit = ciInit;
        }

        public async Task Run()
        {
            await this.ciInit.Run();
        }
    }

    
}