using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cilib;

namespace cicli
    {
    public interface CiCliCommand 
    {
        string CommandLine {get ;}
        Func<Task> Action {get ;}
        string SshCall();
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

        public string SshCall()
        {
            return cli.SshCall(this);
        }
    }
}