using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

public class SshClientWrapper {
    
    SshClient sshClient;

    public SshClientWrapper(SshClient sshClient)
    {
        this.sshClient = sshClient;
    }

    public string RunSudoBash(string command)
    {
        var escapedCommand = command.Replace("\"", "\\\"");
        var commandLine = "bash -c \"" + escapedCommand + "\"";
        return RunSudo(commandLine);
    }

    public string RunSudo(string command)
    {
        var promptRegex = new Regex(@"\][#$>]"); // regular expression for matching terminal prompt
            var modes = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
            using (var stream = sshClient.CreateShellStream("xterm", 255, 50, 800, 600, 1024, modes))
            {
                var output1 = new StreamReader(stream).ReadToEnd();
                stream.WriteLine("sudo " + command);
                var output2 = stream.Expect("Mot de passe");
                var output3 = new StreamReader(stream).ReadToEnd();
                stream.WriteLine("test");
                var output = stream.Expect("test@ubuntu");
                Console.WriteLine(output);
                return output;
            }
    }

    public void SudoReboot()
    {
        // reboot dans 2 seconds
        RunSudo("bash -c \"( sleep 2 ; reboot ) &\"");

        var tries = 0;
        while (sshClient.IsConnected && tries < 50) {
            Thread.Sleep(100);
            tries++;
        }

        if (sshClient.IsConnected)
            throw new Exception("reboot by ssh failed");
    }
}