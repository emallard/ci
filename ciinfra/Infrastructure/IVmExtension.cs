using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.IO;
using System.Text;

public static class IVmExtension {

    public static void Ssh(this IVm vm, Action<SshClient> action)
    {
        using (var client = vm.Ssh())
            action(client);
    }

    public static string SshCommand(this IVm vm, string command)
    {
        using (var client = vm.Ssh())
        {
            var cmd = client.RunCommand(command);
            if (cmd.ExitStatus != 0)
                throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
            return cmd.Result;
        }
    }

    public static string SshSudoBashCommand(this IVm vm, string command)
    {
        using (var client = vm.Ssh())
        {
            var result = new SshClientWrapper(client).RunSudoBash(command);
            return result;
        }
    }

    public static string RunEmbeddedResourceWithSudo(this IVm vm, EmbeddedResource resource) 
    {
        
        using (var scpClient = vm.Scp())
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = vm.Ssh())
        {
            return new SshClientWrapper(client).RunSudo("sh " + resource.Name);
        }
    }

    public static string RunEmbeddedResource(this IVm vm, EmbeddedResource resource) 
    {
        
        using (var scpClient = vm.Scp())
        {
            scpClient.Upload(resource.Stream(), resource.Name);
        }

        using (var client = vm.Ssh())
        {
            var cmd = client.RunCommand("sh " + resource.Name);
            if (cmd.ExitStatus != 0)
                throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
            return cmd.Result;
        }
    }

    public static string SshScript(this IVm vm, string scriptContent, string scriptName) 
    {
        using (var scpClient = vm.Scp())
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(scriptContent));
            scpClient.Upload(stream, scriptName);
        }

        using (var client = vm.Ssh())
        {
            var cmd = client.RunCommand("sh " + scriptName);
            if (cmd.ExitStatus != 0)
                throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
            return cmd.Result;
        }
    }
}