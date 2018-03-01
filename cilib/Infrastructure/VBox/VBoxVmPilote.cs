using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmPilote : IVmPilote {

    Uri sshUri;
    string ip;

    public VBoxVmPilote()
    {
    }

    public void Configure(Uri sshUri, string ip)
    {
        this.sshUri = sshUri;
        this.ip = ip;
    }

    public SshClient Connect()
    {
        var sshClient = new SshClient(GetConnectionInfo());
        sshClient.Connect();
        return sshClient;
    }

    public ScpClient ScpConnect()
    {
        var scpClient = new ScpClient(GetConnectionInfo());
        scpClient.Connect();
        return scpClient;
    }

    public void InstallDocker()
    {
        // How to run sudo commands
        // https://stackoverflow.com/questions/41555597/how-to-run-commands-by-sudo-and-enter-password-by-ssh-net-c-sharp

        var outputInstall1 = RunEmbeddedResourceWithSudo(EmbeddedResources.InstallPilote_1);

        using (var client = Connect())
            new SshClientWrapper(client).SudoReboot();

        // wait for reboot
        Thread.Sleep(20000);
    }

    public void InstallCi()
    {
        var outputInstall2 = RunEmbeddedResourceWithSudo(EmbeddedResources.InstallPilote_2);
    }

    public void CheckInstall()
    {

    }

    private string RunEmbeddedResourceWithSudo(EmbeddedResource resource) 
    {
        
        using (var scpClient = ScpConnect())
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = Connect())
        {
            return new SshClientWrapper(client).RunSudo("sh " + resource.Name);
        }
    }

    private string RunEmbeddedResource(EmbeddedResource resource) 
    {
        
        using (var scpClient = new ScpClient(GetConnectionInfo()))
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = Connect())
        {
            var cmd = client.RunCommand("sh " + resource.Name);
            return cmd.Result;
        }
    }


    private ConnectionInfo GetConnectionInfo()
    {
        var connectionInfo = new ConnectionInfo(
            sshUri.Host, 
            sshUri.Port, 
            "test",
            new PasswordAuthenticationMethod("test", "test"));
        return connectionInfo;
    }

}