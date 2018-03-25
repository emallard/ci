using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using ciinfra;
using ciexecommands;
using citools;

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

                
                var list = getDI().Resolve<CiExeCommands>().CommandList();
                Console.WriteLine(list);
                return;
            }
            
            // initialize vault uri and vaultToken
            Console.Write("uri : ");
            var readVaultUri = Console.ReadLine();
            Console.Write("token : ");
            var readVaultToken = Console.ReadLine();
            getDI().Resolve<Vault>().SetUriAndToken(new Uri(readVaultUri), new VaultToken() { Content = readVaultToken });

            RunSync.Run<CiExeCommands>(getDI(), cli => cli.ExecuteFromCommandLine(args[0]));            
        }


        public static IContainer getDI()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();   
            builder.RegisterType<IVaultSource>().As<VaultSourceTestImpl>();   
                    
            builder.RegisterModule<CiExeCommandsModule>();


            var container = builder.Build();
            return container;
        }
    }
}
