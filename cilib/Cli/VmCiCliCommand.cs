using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public interface IVmCiCliCommand 
{
    string CommandLine {get ;}
    Func<Task> Action {get ;}
    void SshCall();
}

public class VmCiCliCommand<T> : IVmCiCliCommand{

    public string CommandLine {get; set;}
    public Func<Task> Action {get; set;}
    VmCiCli cli;

    public VmCiCliCommand(VmCiCli cli, string commandLine, Func<Task> action)
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