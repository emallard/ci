using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Dynamic;

namespace citools
{
    public class JsonLogger : IAskResourceLogger, IStepLogger
    {

        Func<string, Task> func;

        public JsonLogger()
        {

        }

        public void SetLogFunc(Func<string, Task> func)
        {
            this.func = func;
        }

        public async Task LogAskResource(AskResource askResource)
        {
            dynamic data = new ExpandoObject();
            data.LogType = "Ask";
            data.Name = askResource.Name();
            var jsonData = JsonConvert.SerializeObject(data);
            await func(jsonData);
        }

        public async Task LogCheck(IStep step)
        {
            dynamic data = new ExpandoObject();
            data.LogType = "StepCheck";
            data.Name = step.GetType().Name;
            var jsonData = JsonConvert.SerializeObject(data);
            await func(jsonData);
        }

        public async Task LogClean(IStep step)
        {
            dynamic data = new ExpandoObject();
            data.LogType = "StepClean";
            data.State = "Start";
            data.Name = step.GetType().Name;
            var jsonData = JsonConvert.SerializeObject(data);
            await func(jsonData);

            data.State = "End";
            jsonData = JsonConvert.SerializeObject(data);
            await func(jsonData);

        }

        public async Task LogRun(IStep step)
        {
            dynamic data = new ExpandoObject();
            data.LogType = "StepRun";
            data.Name = step.GetType().Name;
            var jsonData = JsonConvert.SerializeObject(data);
            await func(jsonData);
        }
    }
}