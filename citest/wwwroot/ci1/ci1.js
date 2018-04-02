
let Checking = 0;
let CheckOk = 1;
let CheckException = 2;
let Running = 3;
let RunOk = 4;
let RunException = 5;
let Cleaning = 6;
let CleanOk = 7;
let CleanException = 8;


function stepDataFromMessages(messages, imessage)
{
    /*
    let steps = messages.filter(m => m.Type == "StepLogDto");
    let stepsAndLastState = [];
    for (let step of steps)
    {
        let found = stepsAndLastState.find(s => s.Inner.StepType == step.Inner.StepType);
        if (found != null)
            stepsAndLastState.push(JSON.parse(JSON.stringify(step)));
        else
            found.Inner.StepState = step.Inner.StepState;
    }*/
    let stepData = [];
    for (let i=0; i<imessage; ++i)
    {
        let logDto = messages[i];
        if (logDto.type != "StepLogDto")
            continue;

        let logInner = logDto.inner;
        let found = stepData.find(d => d.stepType == logInner.stepType);
        if (found == null)
            stepData.push({stepType:logInner.stepType, stepState:logInner.stepState, index:stepData.length});
        else
            found.stepState = logInner.stepState;
    }
    for (let i=imessage; i<messages.length; ++i)
    {
        let logDto = messages[i];
        if (logDto.type != "StepLogDto")
            continue;

        let found = stepData.find(d => d.stepType == logDto.inner.stepType);
        if (found == null)
        {
            stepData.push({stepType:logDto.inner.stepType, stepState:-1, index:stepData.length});
        }
    }

    console.log(stepData);
    return stepData;
}


function displayLog(messages, i)
{
    let stepData = stepDataFromMessages(messages, i);

    let svg = d3.select('svg');
    svg.remove();

    svg = d3.select('body').append('svg').attr('width', '800').attr('height', '1500');

    let steps = svg.selectAll("g")
        .data(stepData);

    let elemEnter = steps.enter()
        .append("g")
        .attr("transform", function(d){return "translate(80, " + ((1+d.index)*80) + ")"})

    /*Create the circle for each block */
    let circle = elemEnter.append("rect")
        .attr("width", function(d){return 200} )
        .attr("height", function(d){return 40} )
        
        .attr("stroke", function(d){return colorFromState(d.stepState)})
        .attr("fill", "white")

    /* Create the text for each block */
    elemEnter.append("text")
        .attr("dx", function(d){return 5})
        .attr("dy", function(d){return 20})
        .attr("fill", function(d){return colorFromState(d.stepState)})
        .text(function(d){return d.stepType})

    steps.exit().remove();
}


function colorFromState(state)
{
    if (state == Running)
        return "yellow"; 
    if (state == RunOk)
        return "green";
    if (state == RunException)
        return "red";
    return "black";
}