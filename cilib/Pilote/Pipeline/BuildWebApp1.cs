using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Docker.DotNet.Models;
using System.IO;
using System.Net.Http;
using System.Threading;

public class BuildWebApp1 {

    private readonly DockerWrapper dockerWrapper;
    private readonly GitHelper gitHelper;
    private readonly TarHelper tarHelper;

    string ImageName = "dotnetcore_0";

    public BuildWebApp1(
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
        
        var tag = "1";

        var parameters = new ImageBuildParameters();
        parameters.Tags = new List<string> { ImageName + ":" + tag };

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync("https://raw.githubusercontent.com/emallard/dotnetcore_0/master/Dockerfile");
            File.WriteAllText("../../Dockerfile", response);
        }
        
        tarHelper.CreateTarFile("../../Dockerfile", "../../Dockerfile.tar");
        var tarStream = new FileStream("../../Dockerfile.tar", FileMode.Open);

        var log = await NSwagDockerClientCreate.BuildImageAsync(
            tarStream,
            ImageName + ":" + tag,
            "https://github.com/emallard/dotnetcore_0.git",
            true,
            false
        );


        /* Broken pipe problem with Docker.DotNet
        using (var client = dockerWrapper.GetClient())
        {
            parameters.RemoteContext = "https://github.com/emallard/dotnetcore_0.git";
           
            var stream = await client.Images.BuildImageFromDockerfileAsync(tarStream, parameters);
            Console.WriteLine("coucou");
        }
        */
    }

    public async Task CleanBuild()
    {
        var tag = "1";
        await dockerWrapper.DeleteImageIfExists(ImageName + ":" + tag);
    }

    public async Task Publish()
    {
        // https://stackoverflow.com/questions/28349392/how-to-push-a-docker-image-to-a-private-repository
        //docker tag [OPTIONS] IMAGE[:TAG] [REGISTRYHOST/][USERNAME/]NAME[:TAG]
        //Then docker push using that same tag.
        //docker push NAME[:TAG]
        using (var client = dockerWrapper.GetClient())
        {
            var parameters = new ImageTagParameters();
            parameters.RepositoryName = "localhost:5000/" + ImageName;
            parameters.Tag = "1";
            await client.Images.TagImageAsync(ImageName, parameters);
        }
    }
}