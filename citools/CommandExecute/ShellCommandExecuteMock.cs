using System;
using System.Diagnostics;
using System.IO;

namespace citools
{
    public class ShellCommandExecuteMock : IShellCommandExecute
    {

        public string Command(string command)
        {
            return "";
        }

        public string CommandWithStdIn(string command, string stdIn)
        {
            return "";
        }

        public string Script(string scriptContent, string scriptName)
        {
            return "";
        }

        public void WriteFile(string content, string filename)
        {
        }

        public string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs)
        {
            return "";
        }

        public string SudoBash(string command)
        {
            return "";
        }

        public string SudoScript(string scriptContent, string scriptName)
        {
            return "";
        }
    }
}