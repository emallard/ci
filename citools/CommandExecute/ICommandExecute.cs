using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface ICommandExecute
    {
        string Command(string command);

        string CommandWithStdIn(string command, string stdIn);

        string SudoBash(string command);

        string Script(string scriptContent, string scriptName);

        string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs);

        string SudoScript(string scriptContent, string scriptName);
    }
}