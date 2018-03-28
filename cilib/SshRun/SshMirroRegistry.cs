using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cilib
{
    public class SshMirrorRegistry 
    {
        private readonly ISshClient sshClient;
        
        public SshMirrorRegistry(ISshClient sshClient)
        {
            this.sshClient = sshClient;
        }

        public void InstallMirrorRegistry (SshConnection connection)
        {
            var mirrorPort = 4999;
            // Mirror runs on port 4999 
            var mirrorDir = "/home/test/mirror";
            var cmd = sshClient.Command($"mkdir {mirrorDir}");
            
            cmd = sshClient.Command(
                $"docker run -p {mirrorPort}:5000 -d --restart=always --name registry "
                + $"-e REGISTRY_PROXY_REMOTEURL=http://registry-1.docker.io "
                + $"-v {mirrorDir}:/var/lib/registry "
                + $"registry:2"
            );

            var daemonjson = String.Join("\n", new string[] {
                "{",
                @"    ""registry-mirrors"" : [""http://localhost:" + mirrorPort + @"""]",
                "}"
            });
            var cmdLine = $"echo '{daemonjson}' > /etc/docker/daemon.json";
            sshClient.SudoBash(cmdLine);

            sshClient.SudoReboot();
        }
    }
}