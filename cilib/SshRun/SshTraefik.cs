using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;

namespace cilib
{
    public class SshTraefik {

        private readonly ISshClient sshClient;
        
        public SshTraefik(ISshClient sshClient)
        {
            this.sshClient = sshClient;
        }

        public void InstallTraefik(SshConnection connection)
        {
            var commandline = "docker run -d -p 8080:8080 -p 80:80 -v $PWD/traefik.toml:/etc/traefik/traefik.toml traefik";
            this.sshClient.Connect(connection).Command(commandline);
        }

        public async Task Clean() 
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await dockerWrapper.DeleteContainerIfExistsByName(containerName);
            //await dockerWrapper.DeleteImageIfExists(traefikRepoTag);
        }

    }
}