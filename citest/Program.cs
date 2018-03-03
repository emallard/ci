using System;
using Autofac;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Init<VBoxInfrastructure>();
            var test = container.Resolve<Test>();
            test.RunAll();
        }


        public static IContainer Init<T>() where T : IInfrastructure
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<T>().As<IInfrastructure>();

            builder.RegisterType<ConfigInitDev>().As<IConfigInit>();
            builder.RegisterAssemblyTypes(typeof(Lanceur).Assembly);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly);

            var container = builder.Build();
            return container;
        }
    }
}
