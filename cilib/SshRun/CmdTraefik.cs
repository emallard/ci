using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;
using System.Threading;

namespace cilib
{
    public class CmdTraefik 
    {
        
        public void InstallTraefik(ICommandExecute commandExecute,string tomlPath)
        {
            var traefiktoml = 
@"
[api]

[file]
    filename = ""rules.toml""
";
            var traefikTomlFilename = Path.Combine(tomlPath, "traefik.toml");
            commandExecute.WriteFile(traefiktoml, traefikTomlFilename);
            commandExecute.WriteFile("", Path.Combine(tomlPath, "rules.toml"));
            
            var commandline = "docker run -d -p 8080:8080 -p 80:80 -v "+ traefikTomlFilename +":/etc/traefik/traefik.toml --name=dev-traefik traefik:1.5";
            commandExecute.Command(commandline);
            Thread.Sleep(3000);
        }

        public void Check(ICommandExecute commandExecute) 
        {
            var cmdResponse = commandExecute.Command("curl -s 127.0.0.1:8080");
            StepAssert.Contains("<a href=\"/dashboard/\">", cmdResponse);
        }

        public void Clean(ICommandExecute commandExecute) 
        {
            var commandline = "docker rm -f dev-traefik";
            commandExecute.Command(commandline);
            Thread.Sleep(3000);
        }

    }
}