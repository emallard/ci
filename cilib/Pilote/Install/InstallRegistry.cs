

// https://docs.docker.com/engine/security/https/#create-a-ca-server-and-client-keys-with-openssl

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ciinfra;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace cilib
{
    public class InstallRegistry
    {
        private readonly DockerWrapper dockerWrapper;
        private readonly ShellHelper shellHelper;
        private readonly ICiLibCiDataDirectory cidataDir;
        private readonly IInfrastructure infrastructure;
        private string repoTag = "registry:2";
        private string containerName = "privateregistry";


        public InstallRegistry(
            DockerWrapper dockerWrapper,
            ShellHelper shellHelper,
            ICiLibCiDataDirectory cidataDir,
            IInfrastructure infrastructure)
        {
            this.dockerWrapper = dockerWrapper;
            this.shellHelper = shellHelper;
            this.cidataDir = cidataDir;
            this.infrastructure = infrastructure;
        }

        public async Task Clean()
        {
            await dockerWrapper.DeleteContainerIfExistsByName(containerName);
            // Image is alrady used by the mirror registry
            //await dockerWrapper.DeleteImageIfExists(repoTag); 
        }


        public async Task Install()
        {
            // Image is alrady used by the mirror registry
            await dockerWrapper.CreateImageIfNotFound(repoTag);
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

                // copy tls keys
                shellHelper.Bash($"rm -rf {cidataDir}/privateregistry/certs");
                shellHelper.Bash($"mkdir -p {cidataDir}/privateregistry/certs");
                shellHelper.Bash($"cp {cidataDir}/tls/privateregistry.mynetwork.local.* {cidataDir}/privateregistry/certs");
                

                // Registry data outside the container in /privateregistry
                shellHelper.Bash("mkdir -p {cidataDir}/privateregistry/var/lib/registry");

                
                var infraCidata = "/cidata";//infrastructure.CidataDirectory;
                var p = new CreateContainerParameters();
                p.Image = registryImage.ID;

                p.ExposedPorts = new Dictionary<string, EmptyStruct>();
                p.ExposedPorts.Add("443/tcp", new EmptyStruct());
                
                p.Env = new List<string>()
                {
                    "REGISTRY_HTTP_ADDR=0.0.0.0:443",
                    "REGISTRY_HTTP_TLS_CERTIFICATE=/certs/privateregistry.mynetwork.local.crt",
                    "REGISTRY_HTTP_TLS_KEY=/certs/privateregistry.mynetwork.local.key",
                    "REGISTRY_STORAGE_DELETE_ENABLED=true"
                };
                p.Name = "privateregistry";
                
                p.HostConfig = new DockerHostConfig()
                    .Bind($"{infraCidata}/privateregistry/var/lib/registry:/var/lib/registry")
                    .Bind($"{infraCidata}/privateregistry/certs:/certs")
                    .PortBinding("0.0.0.0", "5443", "443/tcp")
                    .RestartAlways()
                    .GetConfig();

                var response = await client.Containers.CreateContainerAsync(p);

                var p2 = new ContainerStartParameters();
                await client.Containers.StartContainerAsync(response.ID, p2);
            }
        }
    }
}