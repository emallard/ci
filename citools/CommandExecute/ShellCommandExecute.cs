using System;
using System.Diagnostics;
using System.IO;

namespace citools
{
    public class ShellCommandExecute : IShellCommandExecute
    {

        public string Command(string command)
        {
            return this.Bash(command);
        }

        public string CommandWithStdIn(string command, string stdIn)
        {
            return this.BashAndStdIn(command, stdIn);
        }

        public string Script(string scriptContent, string scriptName)
        {
            File.WriteAllText(scriptName, scriptContent);
            return this.ExecScript("/bin/bash", scriptName);
        }

        public void WriteFile(string content, string filename)
        {
            File.WriteAllText(filename, content);
        }

        public string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs)
        {
            throw new NotImplementedException();
        }

        public string SudoBash(string command)
        {
            return this.ExecScript("sudo /bin/bash", command);
        }

        public string SudoScript(string scriptContent, string scriptName)
        {
            File.WriteAllText(scriptName, scriptContent);
            return this.ExecScript("sudo /bin/bash", scriptName);
        }

        // Private :
        private string ExecScript(string filename, string arguments)
        {   
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        private string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\\", "\\\\").Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        private string BashAndStdErr(string cmd, out string stdErr)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            stdErr = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        private string BashAndStdIn(string cmd, string stdIn)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.StandardInput.Write(stdIn);
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}