

using System.Net;
using System.Threading.Tasks;
using Autofac;

public class RunDevelopment 
{
    public async Task Run()
    {
        //HttpWebRequest.DefaultWebProxy = null;
        var container = getDI();
        using (var scope = container.BeginLifetimeScope())
        {
            var init = scope.Resolve<Init>();
            await init.Run();
        }
    }


    public async Task RunWorker()
    {
        //HttpWebRequest.DefaultWebProxy = null;
        var container = getDI();
        using (var scope = container.BeginLifetimeScope())
        {
            var init = scope.Resolve<BuildWorker>();
            await init.Run();
        }
    }

    private IContainer getDI()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<ConfigDev>().As<IConfig>();
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);


        var container = builder.Build();
        return container;
        
    }
}