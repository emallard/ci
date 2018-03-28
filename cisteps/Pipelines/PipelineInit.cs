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
        private readonly CiInit ciInit;

        public PipelineInit(CiInit ciInit)
        {
            this.ciInit = ciInit;
        }

        public async Task Run()
        {
            await this.ciInit.Run();
        }
    }

    
}