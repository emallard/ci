

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

public class InstallRegistry
{
    private readonly DockerWrapper dockerWrapper;
    private readonly ShellHelper shellHelper;
    private string repoTag = "registry:2";
    private string containerName = "privateregistry";


    public InstallRegistry(
        DockerWrapper dockerWrapper,
        ShellHelper shellHelper)
    {
        this.dockerWrapper = dockerWrapper;
        this.shellHelper = shellHelper;
    }

    public async Task Clean()
    {
        await dockerWrapper.DeleteContainerIfExistsByName(containerName);
        await dockerWrapper.DeleteImageIfExists(repoTag); 
    }


    public async Task Install()
    {
        await dockerWrapper.CreateImage(repoTag);
        var registryImage = await dockerWrapper.FindImage(repoTag);

        using (var client = dockerWrapper.GetClient())
        {
            /*
            https://docs.docker.com/registry/deploying/#get-a-certificate

            docker run -d \
            --restart=always \

            docker run \
            --name privateregistry \
            -v ${HOME}/cidata/privateregistry/var/lib/registry:/var/lib/registry \
            -v ${HOME}/cidata/privateregistry/certs:/certs \
            -e REGISTRY_HTTP_ADDR=0.0.0.0:443 \
            -e REGISTRY_HTTP_TLS_CERTIFICATE=/certs/privateregistry.mynetwork.local.crt \
            -e REGISTRY_HTTP_TLS_KEY=/certs/privateregistry.mynetwork.local.key \
            -p 5443:443 \
            registry:2
            */

            shellHelper.Bash("mkdir -p ${HOME}/cidata/privateregistry/var/lib/registry");

            var home = shellHelper.Bash("echo ${HOME}");
            // Registry data will be stored on /cidata/privateregistry

            var p = new CreateContainerParameters();
            p.Image = registryImage.ID;
            p.Volumes = new Dictionary<string, EmptyStruct>();
            p.Volumes.Add(home + "/cidata/privateregistry/var/lib/registry:/var/lib/registry", new EmptyStruct());
            p.Volumes.Add(home + "/cidata/privateregistry/certs:/certs", new EmptyStruct());
            p.ExposedPorts = new Dictionary<string, EmptyStruct>();
            p.ExposedPorts.Add("5443:443", new EmptyStruct());
            p.Env = new List<string>()
            {
                "REGISTRY_HTTP_ADDR=0.0.0.0:443",
                "REGISTRY_HTTP_TLS_CERTIFICATE=/certs/privateregistry.mynetwork.local.crt",
                "REGISTRY_HTTP_TLS_KEY=/certs/privateregistry.mynetwork.local.key"
            };
            p.Name = "privateregistry";
            p.HostConfig = new HostConfig();
            p.HostConfig.RestartPolicy = new RestartPolicy();
            p.HostConfig.RestartPolicy.Name = RestartPolicyKind.Always;
            p.HostConfig.RestartPolicy.MaximumRetryCount = 5;

            var response = await client.Containers.CreateContainerAsync(p);


            var p2 = new ContainerStartParameters();
            await client.Containers.StartContainerAsync(response.ID, p2);
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