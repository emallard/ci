

function receiveLogMessage(msg)
{
    console.log("receiveMessage" , msg);
    if (msh.Type == "StepRunStart")
    {
        //ChangerEtat(msg.StepName);
    }
}