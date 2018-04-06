using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class ConsoleStepLogger : ILogger
    {
        private string indent = "";

        public ConsoleStepLogger()
        {
        }

        public async Task Log(object log)
        {
            await Task.CompletedTask;
            if (log is StepLogDto)
            {
                var l = (StepLogDto) log;
                if (l.StepState == StepState.Entered)
                {
                    Console.WriteLine(l.StepType);
                    indent = "    ";
                }
                
                if (l.StepState == StepState.Checking)
                {
                    Console.WriteLine(indent + "Check ");
                }

                if (l.StepState == StepState.CheckOk)
                {
                    Console.WriteLine(indent + "OK ");
                }

                if (l.StepState == StepState.CheckException)
                {
                    Console.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Running)
                {
                    Console.WriteLine(indent + "Run ");
                }

                if (l.StepState == StepState.RunOk)
                {
                    Console.WriteLine(indent + "Ok ");
                }

                if (l.StepState == StepState.RunException)
                {
                    Console.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Cleaning)
                {
                    Console.WriteLine(indent + "Clean ");
                }

                if (l.StepState == StepState.CleanOk)
                {
                    Console.WriteLine(indent + "Ok ");
                }

                if (l.StepState == StepState.CleanException)
                {
                    Console.WriteLine(indent + "FAIL ");
                }

                if (l.StepState == StepState.Exited)
                {
                    Console.WriteLine();
                    indent = "";
                }
            }

            if (log is AskResourceLogDto)
            {
                var l = (AskResourceLogDto)log;
                Console.WriteLine(indent + "    " + "?? " + l.Name);
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
                Console.WriteLine(indent + "    " + sign + " " + l.Name);
            }
        }
    }
}