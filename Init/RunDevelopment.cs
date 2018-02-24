

using System.Net;
using System.Threading.Tasks;
using Autofac;

public class RunDevelopment 
{
    public async Task Run()
    {
        HttpWebRequest.DefaultWebProxy = null;
        
        var builder = new ContainerBuilder();
        
        builder.RegisterType<ConfigDev>().As<IConfig>();
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);


        var container = builder.Build();
        using (var scope = container.BeginLifetimeScope())
        {
            var init = scope.Resolve<Init>();
            await init.Run();
        }
        
    }
}