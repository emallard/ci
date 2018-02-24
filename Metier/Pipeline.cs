


using System;
using System.IO;
//using Docker.DotNet;
//using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;

using System.Net.Sockets;
using Microsoft.Net.Http.Client;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;

public class Pipeline
{

    public string WorkingDirectory = "./";

    public async Task<int> Lancer()
    {
        await Task.CompletedTask;
        return 1;

        //this.GetResource("sources");
        // lancer un build dans une image docker en mettant /sources dans le volume

        /*
        var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

        var parameters = new ImagesListParameters();
        var list = await client.Images.ListImagesAsync(parameters);

        foreach (var l in list)
        {
            if (l.RepoTags != null)
                Console.WriteLine(l.RepoTags[0]);
        }

        var createParameters = new CreateContainerParameters();
        await client.Containers.CreateContainerAsync(new CreateContainerParameters() {
            Image = "microsoft/aspnetcore-build:2.0",
            Name = "ci_build",
            Volumes = new Dictionary<string,EmptyStruct>() {{"sources:/sources", new EmptyStruct()}}
        });*/
        // récupérer les artefacts

        // produire une image d'éxécution
    }

    public void GetResource(string sourceDirectory)
    {
        var gitResource = new GitResource();
        gitResource.GitRepository = "https://github.com/emallard/dotnetcore_0.git";

        var sourcesDirectory = Path.Combine(WorkingDirectory, sourceDirectory);
        Directory.CreateDirectory(sourcesDirectory);
        gitResource.Get(sourcesDirectory);
    }
}