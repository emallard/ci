using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using cicli;

namespace ciexe
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest.DefaultWebProxy = null;
            if (args.Length == 0) {
                Console.WriteLine("help");
                return;
            }

            if (args[0] == "hello")
            {
                Console.WriteLine("hello");
                return;
            }

            if (args[0] == "help")
            {
                var list = getDI().Resolve<CiCli>().CommandList();
                Console.WriteLine(list);
                return;
            }
            
            RunSync.Run<CiCli>(getDI(), cli => cli.ExecuteFromCommandLine(args[0]));            
        }


        public static IContainer getDI()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<ConfigVBox>().As<IConfig>();
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();
           
            builder.RegisterAssemblyTypes(typeof(CiLib).Assembly);
            builder.RegisterAssemblyTypes(typeof(CiCli).Assembly);

            builder.RegisterInstance<CiDataDirectory>(new CiDataDirectory("/cidata"));

            var container = builder.Build();
            return container;
            
        }
    }
}
