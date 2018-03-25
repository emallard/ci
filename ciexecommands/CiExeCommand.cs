using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciexecommands;

namespace ciexecommands
{
    public interface CiExeCommand 
    {
        string CommandLine {get ;}
        Func<Task> Action {get ;}
        string SshCall();
    }

    public class CiExeCommand<T> : CiExeCommand{

        public string CommandLine {get; set;}
        public Func<Task> Action {get; set;}
        CiExeCommands cli;

        public CiExeCommand(CiExeCommands cli, string commandLine, Func<Task> action)
        {
            this.CommandLine = commandLine;
            this.Action = action;
            this.cli = cli;
        }

        public string SshCall()
        {
            return cli.SshCall(this);
        }
    }
}