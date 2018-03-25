using System;
using Autofac;
using ciexecommands;
using ciinfra;
using ciinit;
using citools;

namespace cicli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                PrintHelp();
                return;
            }

            if (args[0] == "hello")
            {
                Console.WriteLine("hello");
                return;
            }


            if (args[0] == "init")
            {
                getDI().Resolve<CiInit>().Run();
            }

        }

        private static IContainer getDI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();   
            builder.RegisterType<IAsk>().As<AskReadLine>();   
            builder.RegisterModule<CiExeCommandsModule>();

            var container = builder.Build();
            return container;
            
        }


        static void PrintHelp()
        {
var help = @"
- ci init

// ci infra needs {devop-infra token} to be executed
- ci infra pilote create
- ci infra pilote install-docker

// ci admin needs {devop-admin token} to be executed
- ci admin pilote build-ci
- ci admin pilote ci install-vault

- ci admin pilote ci install-ca
- ci admin pilote ci install-private-registry
- ci admin webserver ci install-traefik

- ci admin pilote ci addbuildconf {name} {content}
- ci admin pilote ci adddeployconf {name} {content}

- ci admin pilote ci build {name}
- ci admin webserver ci deploy {name}";

            Console.WriteLine(help);

        }
    }
}
