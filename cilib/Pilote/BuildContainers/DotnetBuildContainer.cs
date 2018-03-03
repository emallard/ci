using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Docker.DotNet.Models;
using System.IO;

public class DotnetBuildContainer {

    private readonly DockerWrapper dockerWrapper;
    private readonly TarHelper tarHelper;
    private readonly GitHelper gitHelper;
    string containerName = "ci_dotnet_build";
    string imageName = "microsoft/aspnetcore-build:2.0";
    string sourcesDirectory = "~/";

    public DotnetBuildContainer(
        DockerWrapper dockerWrapper,
        TarHelper tarHelper,
        GitHelper gitHelper)
    {
        this.dockerWrapper = dockerWrapper;
        this.tarHelper = tarHelper;
        this.gitHelper = gitHelper;
    }

    public void SetContainerName(string containerName)
    {
        this.containerName = containerName;
    }


    public async Task Build(string name, Uri gitUri)
    {
        await this.EnsureBuildContainerCreatedAndRunning();

        
        this.UpdateSources(gitUri, Path.Combine(sourcesDirectory, name));
        await this.CopySourcesToBuildContainer(Path.Combine(sourcesDirectory, name));
        //await this.Compile();
        //await this.ExtractArtifact();
    }



    private async Task EnsureBuildContainerCreatedAndRunning()
    {
        var foundResponse = await this.dockerWrapper.FindContainerByName(this.containerName);
        if (foundResponse == null)
        {
            var parameters = new CreateContainerParameters();
            await this.dockerWrapper.CreateContainer(this.imageName, this.containerName, parameters);
        }

        var startP = new ContainerStartParameters();
        await this.dockerWrapper.EnsureRunning(this.containerName, startP);
    }


    private void UpdateSources(Uri gitUri, string name)
    {
        var dir = Path.Combine(sourcesDirectory, name);
        gitHelper.CloneOrPull(gitUri, dir);
    }


    private async Task CopySourcesToBuildContainer(string name)
    {
        using (var client = this.dockerWrapper.GetClient())
        {   
            var dir = Path.Combine(sourcesDirectory, name);
            var tarFile = Path.Combine(sourcesDirectory,name + ".tar");
            this.tarHelper.CreateTarFile(dir, tarFile);

            var found = await dockerWrapper.FindContainerByName(this.containerName);
            var parameters = new ContainerPathStatParameters();
            await client.Containers.ExtractArchiveToContainerAsync(found.ID, parameters, File.OpenRead(tarFile));
        }
        
    }
}