using System.Net;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Docker.DotNet.Models;
using Docker.DotNet;
using System.Linq;
using System.Threading;

public class DockerWrapper {


    public async Task EnsureRunning(string containerName, ContainerStartParameters parameters)
    {
        /*
        created : A container that has been created (e.g. with docker create) but not started
        restarting : A container that is in the process of being restarted
        running : A currently running container
        paused : A container whose processes have been paused
        exited : A container that ran and completed ("stopped" in other contexts, although a created container is technically also "stopped")
        dead : A container that the daemon tried and failed to stop (usually due to a busy device or resource used by the container)
        */
        using (var client = GetClient())
        {
            var found = await this.FindContainerByName(containerName);
            var state = found.State.ToLowerInvariant();
            if (state == "running") 
                {}
            if (state == "restarting") 
                {}
            if (state == "exited")
                await client.Containers.StartContainerAsync(found.ID, parameters);
            if (state == "paused")
                await client.Containers.UnpauseContainerAsync(found.ID);
            if (state == "dead") 
                {}


            // check that container is really running
            Thread.Sleep(1000);
            found = await this.FindContainerByName(containerName);
            state = found.State.ToLowerInvariant();
            if (state != "running")
                throw new Exception("Container not running : " + containerName);
        }
    }


    public async Task CreateContainer(string repoTag, string containerName, CreateContainerParameters parameters)
    {
        using (var client = GetClient())
        {
            // ensure image exists
            var foundImage = await this.FindImage(repoTag);
            if (foundImage == null)
            {
                await this.CreateImage(repoTag);
                foundImage = await this.FindImage(repoTag);
            }

            parameters.Image = foundImage.ID;
            parameters.Name = containerName;
            await client.Containers.CreateContainerAsync(parameters);
        }
    }



    public async Task DeleteContainerIfExistsByRepoTag(string repoTag)
    {
        var image = await FindImage(repoTag);
        if (image != null)
            await DeleteContainerIfExists(image.ID);
    }

    public async Task<ContainerListResponse> FindContainer(string imageId)
    {
        using (var client = GetClient())
        {
            var p1 = new ContainersListParameters();
            p1.All = true;
            var containers = await client.Containers.ListContainersAsync(p1);
            return containers.FirstOrDefault(i => i.ImageID == imageId);
        }
    }

    public async Task<ContainerListResponse> FindContainerByName(string name)
    {
        using (var client = GetClient())
        {
            var p1 = new ContainersListParameters();
            p1.All = true;
            var containers = await client.Containers.ListContainersAsync(p1);
            return containers.FirstOrDefault(i => i.Names.Contains("/" + name));
        }
    }

    public async Task DeleteContainerIfExists(string imageId)
    {
        var container = await FindContainer(imageId);
        if (container == null)
            return;

        using (var client = GetClient())
        {
            var p = new ContainerRemoveParameters();
            p.Force = true;
            await client.Containers.RemoveContainerAsync(container.ID, p);
        }        
    }

    public async Task DeleteContainerIfExistsByName(string name)
    {
        var container = await FindContainerByName(name);
        if (container == null)
            return;

        using (var client = GetClient())
        {
            var p = new ContainerRemoveParameters();
            p.Force = true;
            await client.Containers.RemoveContainerAsync(container.ID, p);
        }        
    }

    // repotag : "registry:2"
    public async Task<ImagesListResponse> FindImage(string repoTag)
    {
        using (var client = GetClient())
        {
            var p1 = new ImagesListParameters();
            p1.All = true;
            var images = await client.Images.ListImagesAsync(p1);

            return images.FirstOrDefault(i => i.RepoTags != null && i.RepoTags.Any(t => t==repoTag));
        }
    }

    public async Task DeleteImageIfExists(string repoTag)
    {
        var registryImage = await this.FindImage(repoTag);
        if (registryImage == null)
            return;

        using (var client = GetClient())
        {
            await client.Images.DeleteImageAsync(registryImage.ID, new ImageDeleteParameters());
        }
    }



    public async Task CreateImage(string repoTag)
    {
        var registryImage = await this.FindImage(repoTag);
        if (registryImage != null)
            return;

        using (var client = GetClient())
        {
            var parameters = new ImagesCreateParameters();
            var split = repoTag.Split(':');
            parameters.FromImage = split[0];
            parameters.Repo = split[0];
            parameters.Tag = split[1];

            var progress = new DockerProgress(m => {
                if (m.Progress != null) 
                {
                    Console.WriteLine(m.ID + " " + m.ProgressMessage /*+ " : " + m.Progress.Current + "/" + m.Progress.Total*/);
                }
            });
            await client.Images.CreateImageAsync(parameters, null, progress);
        }
    }

    public DockerClient GetClient()
    {
        return new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
    }
}