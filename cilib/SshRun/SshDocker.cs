using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cilib
{
    public class SshDocker 
    {
        private readonly ISshClient sshClient;
        
        public SshDocker(ISshClient sshClient)
        {
            this.sshClient = sshClient;
        }

        public void InstallDocker(SshConnection connection)
        {
            // How to run sudo commands
            // https://stackoverflow.com/questions/41555597/how-to-run-commands-by-sudo-and-enter-password-by-ssh-net-c-sharp

            var outputInstall = this.sshClient.Connect(connection).SudoScript(this.script, "installdocker.sh");

            this.sshClient.SudoReboot();
        }


        string script = @"
set -e -x

sudo apt-get -qq update

sudo apt-get -qq --yes install \
    apt-transport-https \
    ca-certificates \
    curl \
    software-properties-common

sudo add-apt-repository \
    ""deb [arch=amd64] https://download.docker.com/linux/ubuntu \
    $(lsb_release -cs) \
    stable""

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

sudo apt-key fingerprint 0EBFCD88

sudo apt-get -qq update

sudo apt-get -qq --yes install docker-ce

sudo usermod -aG docker test
";

        public void Build(SshConnection connection, string directory)
        {
            this.sshClient.Connect(connection).Command("docker build " + directory);
        }
       
    }
}