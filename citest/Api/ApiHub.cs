using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using cisteps;
using citools;
using Microsoft.AspNetCore.SignalR;

// https://github.com/aspnet/SignalR-samples/

namespace citest
{
    public class ApiHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task RunDoc(string pipelineTypeStr)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommonModule>();
            
            var logger = new FuncLogger().SetFunc(async o => await Clients.All.SendAsync("logMessage", new TypedLogDto(o)));
            builder.RegisterInstance(logger).As<ILogger>();
            //builder.RegisterType<ConsoleLogger>().As<ILogger>();

            builder.RegisterModule<MockModule>();
            builder.RegisterType<StepRunnerRunOnly>().As<IStepRunner>();

            var container = builder.Build();
            
            //container.Resolve<PipelineInstallPiloteVm>();
            //container.Resolve<IStepRunner>();

            var pipelineType = typeof(CiStepsModule).Assembly.GetTypes().First(t => t.Name == pipelineTypeStr);
            var pipeline = (IPipeline) container.Resolve(pipelineType);
            await pipeline.Run();
            Console.WriteLine("end");
        }



/*
        public async Task VBoxSafeRun(string pipelineType) 
        {
            await Task.CompletedTask;
        }

        public async Task VBoxCheck(string pipelineType) 
        {
            await Task.CompletedTask;
        }

        public async Task GandiSafeRun(string pipelineType)
        {
            await Task.CompletedTask;
        }
*/
    }
}