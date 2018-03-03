

using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;

public class Lanceur 
{
    public async Task Run<T>(Func<T, Task> action)
    {
        var container = getDI();
        using (var scope = container.BeginLifetimeScope())
        {
            var instance = scope.Resolve<T>();
            await action(instance);
        }
    }

    public void RunSync<T>(Func<T, Task> action)
    {
        Task.WaitAll(Run<T>(action));
    }

    private IContainer getDI()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);


        var container = builder.Build();
        return container;
        
    }
}