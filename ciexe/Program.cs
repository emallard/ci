﻿using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;

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
                var list= new Lanceur().Resolve<VmCiCli>().CommandList();
                Console.WriteLine(list);
                return;
            }
            
            new Lanceur().RunSync<VmCiCli>(cli => cli.ExecuteFromCommandLine(args[0]));

/*
            if (args[0] == "install-ca")
                new Lanceur().RunSync<InstallCA>(r => r.Install());

            else if (args[0] == "clean-ca")
                new Lanceur().RunSync<InstallCA>(r => r.Clean());

            if (args[0] == "install-privateregistry")
                new Lanceur().RunSync<InstallRegistry>(r => r.Install());

            else if (args[0] == "clean-privateregistry")
                new Lanceur().RunSync<InstallRegistry>(r => r.Clean());
            
            else if (args[0] == "install-vault")
                new Lanceur().RunSync<InstallVault>(r => r.Init());



            else if (args[0] == "build")
                new Lanceur().RunSync<PiloteExample1>(r => r.Build());

            else if (args[0] == "publish")
                new Lanceur().RunSync<PiloteExample1>(r => r.Publish());
*/
            
        }
    }
}
