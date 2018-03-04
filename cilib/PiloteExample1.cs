using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Docker.DotNet.Models;
using System.IO;
using System.Net.Http;

public class PiloteExample1 {

    private readonly DockerWrapper dockerWrapper;
    private readonly GitHelper gitHelper;
    private readonly TarHelper tarHelper;

    public PiloteExample1(
        DockerWrapper dockerWrapper,
        GitHelper gitHelper,
        TarHelper tarHelper)
    {
        this.dockerWrapper = dockerWrapper;
        this.gitHelper = gitHelper;
        this.tarHelper = tarHelper;
    }

    public async Task Build()
    {
        var imageName = "PiloteExample1";
        var tag = "1";

        var parameters = new ImageBuildParameters();
        parameters.Tags = new List<string> { imageName + ":" + tag };

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync("https://raw.githubusercontent.com/emallard/dotnetcore_0/master/Dockerfile");
            File.WriteAllText("Dockerfile", response);
        }
        
        tarHelper.CreateTarFile("Dockerfile", "Dockerfile.tar");
        var tarStream = new FileStream("Dockerfile.tar", FileMode.Open);

        using (var client = dockerWrapper.GetClient())
        {
            parameters.RemoteContext = "https://github.com/emallard/dotnetcore_0.git";
            await client.Images.BuildImageFromDockerfileAsync(tarStream, parameters);
        }
    }

    public async Task BuildManuallyGetSources()
    {
        var imageName = "PiloteExample1";
        var tag = "1";
        
        gitHelper.CloneOrPull(new Uri("https://github.com/emallard/dotnetcore_0.git"), "/sources/dotnetcore_0");
        //var sourcesDirectory = "/sources/dotnetcore_0";
        //var outDirectory = "/out/dotnetcore_0";

        using (var client = dockerWrapper.GetClient())
        {
            var dockerfilePath = "/sources/dotnetcore_0/Dockerfile";
            var tarFilePath = "/sources/dotnetcore_0.tar";

            tarHelper.CreateTarFile(dockerfilePath, tarFilePath);
            var tarStream = new FileStream(tarFilePath, FileMode.Open);

            var parameters = new ImageBuildParameters(); 
            parameters.Tags = new List<string> { imageName + ":" + tag };
            await client.Images.BuildImageFromDockerfileAsync(tarStream, parameters);
        }

    }
}
