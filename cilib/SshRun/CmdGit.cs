using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;

namespace cilib
{
    public class CmdGit {

       
        public async Task CloneOrPull(ICommandExecute execute,  Uri gitUri, string directory) 
        {
            await Task.CompletedTask;
            var cmd = "[ -d \"{directory}\" ] && echo 'Found' || echo 'Not found'";
            if (execute.Command(cmd) == "Not Found")
                Clone(execute, gitUri, directory);
            else
                Pull(execute, directory);
        }

        public void Pull(ICommandExecute execute, string directory)
        {
            var cmd = $"git --git-dir=\"{directory}\" pull";
            execute.Command(cmd);
        }

        public void Clone(ICommandExecute execute, Uri gitUri, string directory)
        {
            var cmd = "git clone " + "\"" + gitUri.ToString() + "\" \"" + directory + "\"";
            execute.Command(cmd);
        }
        

    }
}