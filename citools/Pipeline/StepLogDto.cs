using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public enum StepState
    {
        Entered,
        Checking,
        CheckOk,
        CheckException,
        Running,
        RunOk,
        RunException,
        Cleaning,
        CleanOk,
        CleanException,
        Exited
    }

    public class StepLogDto
    {
        public StepLogDto()
        {
        }
        
        public StepLogDto(string stepType, StepState stepState)
        {
            this.StepType = stepType;
            this.StepState = stepState;
        }

        public string StepType {get; set;}
        public StepState StepState {get; set;}
    }
}