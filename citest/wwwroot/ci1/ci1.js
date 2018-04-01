

function displayLog(messages, i)
{
    var svg = document.querySelector('#svg');
    console.log("displayLog " + i);

    var msg = messages[i];

    if (msg.Type == "AskResourceLogDto")
    {
        var m = msg.Inner;
        //ChangerEtat(msg.StepName);

        ajouterTexteDans(svg, '#ask', m.Name);
    }

    if (msg.Type == "StepLogDto")
    {
        var m = msg.Inner;
        if (m.StepState == Running)
        {
            bordureTexte(svg, '#'+ m.StepType, "yellow")
        }
        if (m.StepState == RunningOk)
        {
            bordureTexte(svg, '#'+ m.StepType, "green")
        }
        if (m.StepState == RunningException)
        {
            bordureTexte(svg, '#'+ m.StepType, "red")
        }
    }
}




var Checking = 0;
var CheckOk = 1;
var CheckException = 2;
var Running = 3;
var RunOk = 4;
var RunException = 5;
var Cleaning = 6;
var CleanOk = 7;
var CleanException = 8;