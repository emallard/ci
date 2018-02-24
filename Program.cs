using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ci
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var run = new RunDevelopment();
            var t = run.Run();

            System.Threading.Tasks.Task.WaitAll(t);
            //var task = (new Pipeline().Lancer());
            //Task.WaitAll(task);
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
