using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace citools
{
    public class ConsoleAndFileStepLogger : ILogger
    {
        private string indent = "";
        private string filename = "ciTestLog";

        public ConsoleAndFileStepLogger()
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private void WriteLine(string s)
        {
            Console.WriteLine(s);
            File.AppendAllText(filename, s + "\n");
        }

        public async Task Log(object log)
        {
            await Task.CompletedTask;
            if (log is StepLogDto)
            {
                var l = (StepLogDto) log;
                if (l.StepState == StepState.Entered)
                {
                    this.WriteLine(l.StepType);
                    indent = "    ";
                }
                
                if (l.StepState == StepState.Checking)
                {
                    this.WriteLine(indent + "Check ");
                }

                if (l.StepState == StepState.CheckOk)
                {
                    this.WriteLine(indent + "OK ");
                }

                if (l.StepState == StepState.CheckException)
                {
                    this.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Running)
                {
                    this.WriteLine(indent + "Run ");
                }

                if (l.StepState == StepState.RunOk)
                {
                    this.WriteLine(indent + "Ok ");
                }

                if (l.StepState == StepState.RunException)
                {
                    this.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Cleaning)
                {
                    this.WriteLine(indent + "Clean ");
                }

                if (l.StepState == StepState.CleanOk)
                {
                    this.WriteLine(indent + "Ok ");
                }

                if (l.StepState == StepState.CleanException)
                {
                    this.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Exited)
                {
                    this.WriteLine("");
                    indent = "";
                }
            }

            if (log is AskResourceLogDto)
            {
                var l = (AskResourceLogDto)log;
                this.WriteLine(indent + "    " + "?? " + l.Name);
            }

            if (log is StoreResourceLogDto)
            {
                var l = (StoreResourceLogDto)log;
                var sign = "";
                if (l.State == StoreResourceLogDtoState.Read) 
                    sign = "->";
                if (l.State == StoreResourceLogDtoState.Write) 
                    sign = "<-";
                if (l.State == StoreResourceLogDtoState.Delete) 
                    sign = "xx";
                this.WriteLine(indent + "    " + sign + " " + l.Name);
            }
        }
    }
}