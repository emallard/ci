using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

public class RunSync {

    public static void Run<T>(IContainer container, Func<T, Task> action)
    {
        Task.WaitAll(_Run<T>(container, action));
    }

    private static async Task _Run<T>(IContainer container, Func<T, Task> action)
    {
        using (var scope = container.BeginLifetimeScope())
        {
            var instance = scope.Resolve<T>();
            Console.WriteLine("Lanceur");
            await action(instance);
            Console.WriteLine("Lanceur Ok");
        }
    }
}