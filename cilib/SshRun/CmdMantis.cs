using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace cilib
{
    public class CmdMantis 
    {
        public void InstallMantisTls(ICommandExecute execute)
        {
            // get dockerfile from git

            var volumes = "";
            var env = "";
            var commandline = "docker run -d " + env + volumes + " -p 8200:8200 --name=dev-mantis vimagick/mantisbt";
            Console.WriteLine(commandline);
            execute.Command(commandline);
        }

        public void CleanMantisNoTls(ICommandExecute execute)
        {
            var commandline = "docker rm -f dev-mantis";
            execute.Command(commandline);
            Thread.Sleep(3000);
        }
        
        public void Check(ICommandExecute execute)
        {
        }
    }
}