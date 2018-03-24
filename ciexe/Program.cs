using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using cicli;
using ciinfra;
using cilib;

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
                // initialize vaultToken
                Console.Write("token : ");
                var readVaultToken = Console.ReadLine();
                getDI().Resolve<Vault>().SetToken(new VaultToken() { Content = readVaultToken });
                var list = getDI().Resolve<CiCli>().CommandList();
                Console.WriteLine(list);
                return;
            }
            
            RunSync.Run<CiCli>(getDI(), cli => cli.ExecuteFromCommandLine(args[0]));            
        }


        public static IContainer getDI()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();   
            builder.RegisterType<IVaultBackend>().As<VaultBackendTestImpl>();   
                    
            builder.RegisterModule<CiCliModule>();


            var container = builder.Build();
            return container;
            
        }
    }
}
