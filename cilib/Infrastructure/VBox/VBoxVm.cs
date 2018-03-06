using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVm : IVm {

    Uri sshUri;
    string ip;

    public VBoxVm()
    {
    }

    public void Configure(Uri sshUri, string ip)
    {
        this.sshUri = sshUri;
        this.ip = ip;
    }

    public SshClient Ssh()
    {
        var sshClient = new SshClient(GetConnectionInfo());
        sshClient.Connect();
        return sshClient;
    }

    public ScpClient Scp()
    {
        var scpClient = new ScpClient(GetConnectionInfo());
        scpClient.Connect();
        return scpClient;
    }

    public void InstallDocker()
    {
        // How to run sudo commands
        // https://stackoverflow.com/questions/41555597/how-to-run-commands-by-sudo-and-enter-password-by-ssh-net-c-sharp

        var outputInstall1 = this.RunEmbeddedResourceWithSudo(EmbeddedResources.InstallDocker);

        using (var client = Ssh())
            new SshClientWrapper(client).SudoReboot();

        // wait for reboot
        Thread.Sleep(20000);
    }


    public void InstallMirrorRegistry()
    {
        using (var client = Ssh())
        {
            var mirrorPort = 4999;
            // Mirror runs on port 4999 
            var mirrorDir = "/home/test/mirror";
            var cmd = client.RunCommand($"mkdir {mirrorDir}");
            var wait = cmd.Result;
            
            cmd = client.RunCommand(
                  $"docker run -p {mirrorPort}:5000 -d --restart=always --name registry "
                + $"-e REGISTRY_PROXY_REMOTEURL=http://registry-1.docker.io "
                + $"-v {mirrorDir}:/var/lib/registry "
                + $"registry:2"
            );
            wait = cmd.Result;
            

            var daemonjson = String.Join("\n", new string[] {
                "{",
                @"    ""registry-mirrors"" : [""http://localhost:" + mirrorPort + @"""]",
                "}"
            });
            var cmdLine = $"echo '{daemonjson}' > /etc/docker/daemon.json";
            wait = new SshClientWrapper(client).RunSudoBash(cmdLine);

            new SshClientWrapper(client).SudoReboot();
        }

        Thread.Sleep(20000);
    }


    public void CloneOrPullCiSources()
    {
        this.SshSudoBashCommand("sudo apt-get -qq --yes install git");
        
        var script = @"
        set -e -x
        if [ -d ""~/ci"" ]; then
            git pull --quiet
        else
            mkdir ~/ci
            git --git-dir=~/ci clone --quiet https://github.com/emallard/ci.git
        fi
        ";
        this.SshScript(script, "InstallCiSources.sh");
    }

    public void CleanCiSources()
    {
        this.Ssh(client => new SshClientWrapper(client).RunSudoBash("rm -rf ~/ci"));
    }

    public void BuildCiImage()
    {
        this.SshCommand("docker build --force-rm -t ciexe ~/ci");
        this.SshCommand("docker image rm $(docker images -f \"dangling=true\" -q)");
    }


    public void CleanCiImage()
    {
        var containers = this.SshCommand("docker ps -a");
        if (containers.Contains("ciexe"))
            this.SshCommand("docker rm -f ciexe");
        
        var images = this.SshCommand("docker images");
        if (images.Contains("ciexe"))
            this.SshCommand("docker image rm -f ciexe");
    }

#region Alternative build
    public void InstallDotNetCoreSdk()
    {
        this.SshCommand("curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg");
        this.SshSudoBashCommand("mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg");
        this.SshSudoBashCommand("sh -c 'echo \"deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main\" > /etc/apt/sources.list.d/dotnetdev.list'");

        this.SshSudoBashCommand("sudo apt-get -qq --yes install apt-transport-https");
        this.SshSudoBashCommand("sudo apt-get -qq update");
        this.SshSudoBashCommand("sudo apt-get -qq --yes install dotnet-sdk-2.1.4");
    }

    public void BuildCiUsingSdk()
    {
        this.SshCommand("cd ci && dotnet restore && dotnet publish -c Release -o out");
        this.SshCommand("cd ci && docker build -f DockerfileLocalBuild -t ciexe ~/ci");
    }
#endregion

    protected ConnectionInfo GetConnectionInfo()
    {
        var connectionInfo = new ConnectionInfo(
            sshUri.Host, 
            sshUri.Port, 
            "test",
            new PasswordAuthenticationMethod("test", "test"));
        return connectionInfo;
    }

}