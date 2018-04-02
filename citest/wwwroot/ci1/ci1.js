function displayLog(messages, i)
{
    let svg = d3.select('#mysvg');
    svg.selectAll("*").remove();
    //svg = d3.select('.svg-inner2').append('svg').attr('class', 'mysvg').attr('width', '1000').attr('height', '1000');

    let stepData = stepDataFromMessages(messages, i);
    displaySteps(svg, stepData);


    let askData = askDataFromMessages(messages, i);
    displayAskData(svg, askData);
}


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


function askDataFromMessages(messages, imessage)
{
    let askData = [];
    for (let i=0; i<imessage; ++i)
    {
        let logDto = messages[i];
        if (logDto.type != "AskResourceLogDto")
            continue;

        let logInner = logDto.inner;
        let found = askData.find(d => d.name == logInner.name);
        if (found == null)
            askData.push({name:logInner.name, index:askData.length});
    }
    return askData;
}




function displaySteps(svg, stepData)
{
    let steps = svg.selectAll("g.step")
        .data(stepData);

    let elemEnter = steps.enter()
        .append("g")
        .attr("class", "step")
        .attr("transform", function(d){return "translate(80, " + ((1+d.index)*80) + ")"})

    /*Create the circle for each block */
    let circle = elemEnter.append("rect")
        .attr("width", 200 )
        .attr("height", 40 )
        
        .attr("stroke", function(d){return colorFromState(d.stepState)})
        .attr("fill", "white")

    /* Create the text for each block */
    elemEnter.append("text")
        .attr("dx", 5)
        .attr("dy", 20)
        .attr("fill", function(d){return colorFromState(d.stepState)})
        .text(function(d){return d.stepType})

    steps.exit().remove();
}


function colorFromState(state)
{
    if (state == Running)
        return "orange"; 
    if (state == RunOk)
        return "green";
    if (state == RunException)
        return "red";
    return "black";
}




function displayAskData(svg, askData)
{
    let steps = svg.selectAll("g.ask")
        .data(askData);

    let elemEnter = steps.enter()
        .append("g")
        .attr("class", "ask")
        .attr("transform", function(d){return "translate(350, " + (50 + d.index*30) + ")"})

    /* Create the text for each block */
    elemEnter.append("text")
        .attr("dx", 5)
        .attr("dy", 20)
        .attr("fill", "black")
        .text(function(d){return d.name})

    steps.exit().remove();
}