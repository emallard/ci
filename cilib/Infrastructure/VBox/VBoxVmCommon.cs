using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmCommon {

    Uri sshUri;
    string ip;

    public VBoxVmCommon()
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

        var outputInstall1 = RunEmbeddedResourceWithSudo(EmbeddedResources.InstallDocker);

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

    public void InstallCi()
    {
        var outputInstall2 = RunEmbeddedResourceWithSudo(EmbeddedResources.InstallCi);
    }

    protected string RunEmbeddedResourceWithSudo(EmbeddedResource resource) 
    {
        
        using (var scpClient = Scp())
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = Ssh())
        {
            return new SshClientWrapper(client).RunSudo("sh " + resource.Name);
        }
    }

    protected string RunEmbeddedResource(EmbeddedResource resource) 
    {
        
        using (var scpClient = new ScpClient(GetConnectionInfo()))
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = Ssh())
        {
            var cmd = client.RunCommand("sh " + resource.Name);
            return cmd.Result;
        }
    }


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