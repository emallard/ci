

// https://docs.docker.com/engine/security/https/#create-a-ca-server-and-client-keys-with-openssl

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

public class InitRegistry
{
    private readonly IConfig config;



    public InitRegistry(IConfig config)
    {
        this.config = config;
        var ip = config.PiloteIp;
        Console.WriteLine(ip);
    }

    public async Task Init()
    {
        
        await InitRegistryImage();
        await InitRegistryContainer();
    }

    public async Task InitRegistryImage()
    {
        var registryImage = await this.FindImageRegistryLocally();
        if (registryImage != null)
            return;

        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            var parameters = new ImagesCreateParameters();
            parameters.FromImage = "registry";
            parameters.Repo = "registry";
            parameters.Tag = "2";

            var progress = new DockerProgress(m => {
                if (m.Progress != null) 
                {
                    Console.WriteLine(m.ID + " " + m.ProgressMessage /*+ " : " + m.Progress.Current + "/" + m.Progress.Total*/);
                }
            });
            await client.Images.CreateImageAsync(parameters, null, progress);
        }
    }

    public async Task CleanRegistryImage()
    {
        var registryImage = await this.FindImageRegistryLocally();
        if (registryImage == null)
            return;

        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            await client.Images.DeleteImageAsync(registryImage.ID, new ImageDeleteParameters());
        }
    }


    public async Task<ImagesListResponse> FindImageRegistryLocally()
    {
        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            var p1 = new ImagesListParameters();
            p1.All = true;
            var images = await client.Images.ListImagesAsync(p1);

            return images.FirstOrDefault(i => i.RepoTags != null && i.RepoTags.Any(t => t=="registry:2"));
        }
    }

    public async Task InitRegistryContainer()
    {
        var registryContainer = await this.FindContainerRegistryLocally();
        if (registryContainer != null)
            return;

        var registryImage = await this.FindImageRegistryLocally();
        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            /*
            https://docs.docker.com/registry/deploying/#get-a-certificate

            docker run -d \
            --restart=always \
            --name registry \
            -v `pwd`/certs:/certs \
            -e REGISTRY_HTTP_ADDR=0.0.0.0:443 \
            -e REGISTRY_HTTP_TLS_CERTIFICATE=/certs/domain.crt \
            -e REGISTRY_HTTP_TLS_KEY=/certs/domain.key \
            -p 443:443 \
            registry:2
            */

            var p = new CreateContainerParameters();
            p.Image = registryImage.ID;
            p.Volumes = new Dictionary<string, EmptyStruct>();
            p.Volumes.Add("/certs:/certs", new EmptyStruct());
            p.ExposedPorts = new Dictionary<string, EmptyStruct>();
            p.ExposedPorts.Add("443:443", new EmptyStruct());
            p.Env = new List<string>()
            {
                "REGISTRY_HTTP_ADDR=0.0.0.0:443",
                "REGISTRY_HTTP_TLS_CERTIFICATE=/certs/domain.crt",
                "REGISTRY_HTTP_TLS_KEY=/certs/domain.key"
            };
            await client.Containers.CreateContainerAsync(p);
        }
    }

    public async Task CleanRegistryContainer()
    {
        var container = await FindContainerRegistryLocally();
        if (container == null)
            return;

        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            var p = new ContainerRemoveParameters();
            p.Force = true;
            await client.Containers.RemoveContainerAsync(container.ID, p);
        }        
    }

    
    public async Task<ContainerListResponse> FindContainerRegistryLocally()
    {
        var registryImage = await this.FindImageRegistryLocally();
        if (registryImage == null)
            return null;

        using (var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
        {
            var p1 = new ContainersListParameters();
            p1.All = true;
            var containers = await client.Containers.ListContainersAsync(p1);
            return containers.FirstOrDefault(i => i.ImageID == registryImage.ID);
        }
    }

}