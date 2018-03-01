


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
using Docker.DotNet.Models;

// Le worker est lancé dans son propre conteneur docker pour dotnetcore
public class BuildWorker
{

    public string WorkingDirectoryInHost = "/home/etienne/docker/workerfiles";
    
    private readonly DockerWrapper dockerWrapper;
    private readonly ShellHelper shellHelper;

    public BuildWorker(
        DockerWrapper dockerWrapper,
        ShellHelper shellHelper) 
    {
        this.dockerWrapper = dockerWrapper;
        this.shellHelper = shellHelper;
    }

    public async Task Run()
    {
        var repoTag = "microsoft/aspnetcore-build:2.0";
        var image = await dockerWrapper.FindImage(repoTag);
        if (image == null)
        {
            await dockerWrapper.CreateImage(repoTag);
            image = await dockerWrapper.FindImage(repoTag);
        }
        
        //GetResource(WorkingDirectoryInHost);
        await BuildArtifact();

            // Récupérer le répertoire .out, et builder une image de prod
            /*
            FROM microsoft/aspnetcore:2.0
            WORKDIR /app
            COPY --from=build-env /app/out .
            ENTRYPOINT ["dotnet", "1_EntityFramework.dll"]
            */
            /*
            var p = new CreateContainerParameters();
            p.Image = image.ID;
            p.Volumes = new Dictionary<string,EmptyStruct>() {
                {"sources:/sources", new EmptyStruct()},
                {"out:/out", new EmptyStruct()}
                };
            p.Name = "ci_dotnet_build";
            */
        //await client.Images.BuildImageFromDockerfileAsync();
    }

    public void GetResource(string sourceDirectory)
    {
        //"apt-get install -y git".Bash();
        var gitResource = new GitResource();
        gitResource.GitRepository = "https://github.com/emallard/dotnetcore_0.git";

        //Directory.CreateDirectory(sourcesDirectory);
        gitResource.Get(sourceDirectory);
    }

    private async Task BuildArtifact2()
    {
        var p = new ImageBuildParameters();
        p.Dockerfile = "Dockerfile";
        p.Tags = new List<string> { "dotnetcore_0"};
        await dockerWrapper.GetClient().Images.BuildImageFromDockerfileAsync(
            new FileStream("/home/etienne/docker/workerfiles/dotnetcore_0/Dockerfile", FileMode.Open),
            p);
    }

    private async Task BuildArtifact()
    {

        await dockerWrapper.DeleteContainerIfExistsByName("ci_dotnet_build");

        var repoTag = "microsoft/aspnetcore-build:2.0";
        var image = await dockerWrapper.FindImage(repoTag);
        using (var client = dockerWrapper.GetClient())
        {
            var p = new CreateContainerParameters();
            p.Image = image.ID;
            /*
            p.Volumes = new Dictionary<string,EmptyStruct>() {
                {"sources:/sources", new EmptyStruct()},
                {"out:/out", new EmptyStruct()}
                };
            */
            p.Name = "ci_dotnet_build";
            var container = await client.Containers.CreateContainerAsync(p);


            var startP = new ContainerStartParameters();
            await client.Containers.StartContainerAsync(container.ID, startP);

            // copy sources to image 
            var tarFilePath = WorkingDirectoryInHost + "/sources.tar";
            if (File.Exists(tarFilePath))
                File.Delete(tarFilePath);

            shellHelper.Bash("tar cvf " + tarFilePath + " " + WorkingDirectoryInHost + "/dotnetcore_0");
            var parameters = new ContainerPathStatParameters();
            parameters.AllowOverwriteDirWithFile = true;
            parameters.Path = "/sources";
            var stream = new FileStream(tarFilePath, FileMode.Open);
            await client.Containers.ExtractArchiveToContainerAsync(container.ID, parameters, stream);
            
            /*
            var startP = new ContainerStartParameters();
            await client.Containers.StartContainerAsync(response.ID, startP);
            */

            /*
            var eConfig = new ContainerExecStartParameters();
            eConfig.Cmd = new List<string>() {
                "dotnet restore",
                "dotnet publish -c Release -o /out"};
            await client.Containers.StartWithConfigContainerExecAsync(container.ID, eConfig);


            var getParams = new GetArchiveFromContainerParameters();
            getParams.Path = "/out.tar";
            var response = await client.Containers.GetArchiveFromContainerAsync(container.ID, getParams, false);
            response.Stream.CopyTo(new FileStream(WorkingDirectoryInHost + "", FileMode.OpenOrCreate));
            */
        }
    }
}