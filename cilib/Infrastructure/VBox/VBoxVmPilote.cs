using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;

public class VBoxVmPilote : IVmPilote {

    
    private readonly ResourceHelper resourceHelper;
    Uri sshUri;
    string ip;

    public VBoxVmPilote(ResourceHelper resourceHelper)
    {
        this.resourceHelper = resourceHelper;
    }

    public void Configure(Uri sshUri, string ip)
    {
        this.sshUri = sshUri;
        this.ip = ip;
    }

    public SshClient Connect()
    {
        return new SshClient(GetConnectionInfo());
    }

    public void Install()
    {
        // How to run sudo commands
        // https://stackoverflow.com/questions/41555597/how-to-run-commands-by-sudo-and-enter-password-by-ssh-net-c-sharp

        RunEmbeddedResource(EmbeddedResources.InstallPilote_1);

        // wait for reboot
        Thread.Sleep(30000);

        RunEmbeddedResource(EmbeddedResources.InstallPilote_2);
    }

    public void CheckInstall()
    {

    }

    private string RunEmbeddedResourceWithSudo(EmbeddedResource resource) 
    {
        
        using (var scpClient = new ScpClient(GetConnectionInfo()))
        {
            scpClient.Upload(resourceHelper.Read(resource), resource.Name);
        }

        using (var client = Connect())
        {
            var promptRegex = new Regex(@"\][#$>]"); // regular expression for matching terminal prompt
            var modes = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
            using (var stream = client.CreateShellStream("xterm", 255, 50, 800, 600, 1024, modes))
            {
                //stream.Write("sudo sh " + resource.Name);
                stream.Write("sudo echo coucou");
                stream.Expect(":");
                stream.Write("test\n");
                var output = stream.Expect(promptRegex);
                return output;
            }
            
        }
    }

    private string RunEmbeddedResource(EmbeddedResource resource) 
    {
        
        using (var scpClient = new ScpClient(GetConnectionInfo()))
        {
            scpClient.Upload(resourceHelper.Read(resource), resource.Name);
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