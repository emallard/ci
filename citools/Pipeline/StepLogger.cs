using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepLogger
    {
        private readonly ILogger logger;

        public StepLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task LogRun(IStep step)
        {
            await logger.Log(new StepLogDto(step.GetType().Name, StepState.Running));
            try 
            {
                await step.Run();
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.RunOk));
            }
            catch (Exception e)
            {
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.RunException));
                throw e;
            }
            
        }

        public async Task LogCheck(IStep step)
        {
            await logger.Log(new StepLogDto(step.GetType().Name, StepState.Checking));
            try 
            {
                await step.Check();
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.CheckOk));
            }
            catch (Exception e)
            {
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.CheckException));
                throw e;
            }
        }

        public async Task LogClean(IStep step)
        {
            await logger.Log(new StepLogDto(step.GetType().Name, StepState.Cleaning));
            try 
            {
                await step.Clean();
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.CleanOk));
            }
            catch (Exception e)
            {
                await logger.Log(new StepLogDto(step.GetType().Name, StepState.CleanException));
                throw e;
            }
        }
    }
}