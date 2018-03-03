using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class Test {

    public Action RunAll;

    public Test(
        IInfrastructure infrastructure,
        CreateVmPilote createVmPilote,
        CreateVmWebServer createVmWebServer)
    {
        RunAll = () =>
        {
            infrastructure.TryToStartVmPilote();
            infrastructure.TryToStartVmWebServer();

            Run(createVmPilote);
            Run(createVmWebServer);
        };
    }    

    
    private void Run(IStep step)
    {
        try {
            step.Test();
        }
        catch (AssertException)
        {
            step.Revert();
            step.Run();
            step.Test();
        }
    }
}