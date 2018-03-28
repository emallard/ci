using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cilib
{
    public class SshCiexe 
    {
        private readonly ISshClient sshClient;
        
        public SshCiexe(ISshClient sshClient)
        {
            this.sshClient = sshClient;
        }

        public void CloneOrPullCiSources(SshConnection connection)
        {
            sshClient.Connect(connection);
            sshClient.SudoBash("sudo apt-get -qq --yes install git");
            
            var script = @"
            set -e -x
            if [ -d ""${HOME}/ci"" ]; then
                cd ${HOME}/ci
                git pull --quiet
            else
                mkdir ~/ci
                git --git-dir=${HOME}/ci clone --quiet https://github.com/emallard/ci.git
            fi
            ";
            sshClient.Script(script, "InstallCiSources.sh");
        }

        public void CleanCiSources(SshConnection connection)
        {
            sshClient.Connect(connection);
            sshClient.SudoBash("rm -rf ~/ci");
        }

        public void BuildCiImage(SshConnection connection)
        {
            sshClient.Connect(connection);
            sshClient.Command("docker build --force-rm -t ciexe ~/ci");
            sshClient.Command("docker image rm $(docker images -f \"dangling=true\" -q)");
        }


        public void CleanCiImage(SshConnection connection)
        {
            sshClient.Connect(connection);
            var containers = sshClient.Command("docker ps -a");
            if (containers.Contains("ciexe"))
                sshClient.Command("docker rm -f ciexe");
            
            var images = sshClient.Command("docker images");
            if (images.Contains("ciexe"))
                sshClient.Command("docker image rm -f ciexe");
        }

#region Alternative build
        public void InstallDotNetCoreSdk(SshConnection connection)
        {
            sshClient.Connect(connection);
            sshClient.Command("curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg");
            sshClient.SudoBash("mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg");
            sshClient.SudoBash("sh -c 'echo \"deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main\" > /etc/apt/sources.list.d/dotnetdev.list'");

            sshClient.SudoBash("sudo apt-get -qq --yes install apt-transport-https");
            sshClient.SudoBash("sudo apt-get -qq update");
            sshClient.SudoBash("sudo apt-get -qq --yes install dotnet-sdk-2.1.4");
        }

        public void BuildCiUsingSdk(SshConnection connection)
        {
            sshClient.Connect(connection);
            sshClient.Command("cd ci && dotnet restore && dotnet publish -c Release -o out");
            sshClient.Command("cd ci && docker build --force-rm -f DockerfileLocalBuild -t ciexe ~/ci");
        }
#endregion
    }
}