

// https://docs.docker.com/engine/security/https/#create-a-ca-server-and-client-keys-with-openssl

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

public class InitRegistry
{
    private readonly DockerWrapper dockerWrapper;

    private string repoTag = "registry:2";
    private string containerName = "my-registry";


    public InitRegistry(
        DockerWrapper dockerWrapper)
    {
        this.dockerWrapper = dockerWrapper;
    }

    public async Task CleanRegistryImage()
    {
        await dockerWrapper.DeleteImageIfExists(repoTag);
    }

    public async Task CleanRegistryContainer()
    {
        await dockerWrapper.DeleteContainerIfExistsByName(containerName); 
    }


    public async Task Init()
    {
        
        await InitRegistryImage();
        await InitRegistryContainer();
    }

    public async Task InitRegistryImage()
    {
         await dockerWrapper.CreateImage(repoTag);
    }

    public async Task InitRegistryContainer()
    {
/* 
        var registryContainer = await dockerWrapper.FindContainerByName(containerName);
        if (registryContainer != null)
            return;
*/
        var registryImage = await dockerWrapper.FindImage(repoTag);
        using (var client = dockerWrapper.GetClient())
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

/*
    public async Task AddTLSKey()
    {
        
        // Ask Certificate Authority for files

        // Create tarball with keys
        var tarStream = new MemoryStream();
        
        //using (Stream gzipStream = new GZipOutputStream(fs))
        using (TarArchive tarArchive = TarArchive.CreateOutputTarArchive(tarStream))
        {
            TarEntry tarEntry = TarEntry.CreateEntryFromFile("private.key");
            tarArchive.WriteEntry(tarEntry, true);
        }


        tarStream.Position = 0;
        using (var client = dockerWrapper.GetClient())
        {
            var parameters = new ContainerPathStatParameters();
            var response = await dockerWrapper.FindContainerByName(this.containerName);
            await client.Containers.ExtractArchiveToContainerAsync(response.ID, parameters, tarStream);
        }
    
        tarStream.Dispose();
        
        // Copy to container
    }
*/
}