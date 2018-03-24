using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using ciinfra;

namespace cilib
{
    public class TarHelper {

        private readonly ShellHelper shellHelper;

        public TarHelper(ShellHelper shellHelper)
        {
            this.shellHelper = shellHelper;
        }

        public void CreateTarFile(string path,string outputFile)
        {
            var cmd = $"tar -cf {outputFile} " + path;
            this.shellHelper.Bash(cmd);
        }

        public Stream CreateTarStream(string path)
        {
            var cmd = "tar -cf - " + path;
            var escapedArgs = cmd.Replace("\"", "\\\"");

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
            return process.StandardOutput.BaseStream;        
        }
    }
}