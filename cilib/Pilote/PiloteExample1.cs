using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class PiloteExample1 {

    private readonly DotnetBuildContainer buildDotnetCore_0;

    public PiloteExample1(
        DotnetBuildContainer buildDotnetCore_0
    )
    {
        this.buildDotnetCore_0 = buildDotnetCore_0;
        this.buildDotnetCore_0.SetContainerName("ci_dotnet_build");
    }


    public async Task UpdateBuilds()
    {
        await Task.CompletedTask;
        /*
        await this.buildDotnetCore_0.Build(
            "dotnetcore_0",
            "https://github.com/emallard/dotnetcore_0.git");
        */
    }
}