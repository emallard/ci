using System.Net;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Docker.DotNet.Models;
using Docker.DotNet;
using System.Linq;

public class DockerWrapper {


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
            var split = repoTag.Split(":");
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