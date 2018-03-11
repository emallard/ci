using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cicli
    {
    public interface CiCliCommand 
    {
        string CommandLine {get ;}
        Func<Task> Action {get ;}
        void SshCall();
    }

    public class CiCliCommand<T> : CiCliCommand{

        public string CommandLine {get; set;}
        public Func<Task> Action {get; set;}
        CiCli cli;

        public CiCliCommand(CiCli cli, string commandLine, Func<Task> action)
        {
            this.CommandLine = commandLine;
            this.Action = action;
            this.cli = cli;
        }

        public void SshCall()
        {
            cli.SshCall(this);
        }
    }
}