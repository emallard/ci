using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace cisystem
{
    
    public class CiSystem
    {
        public CiWebServer WebServer;
        public CiPilote Pilote;
        CiSystemConfig config;

        public void Configure(CiSystemConfig config)
        {
            this.config = config;
        }

        public void CheckPrivateRegistry(CiPilote pilote)
        {

        }

        public void CheckPrivateRegistryConnection(CiWebServer webserver, CiPilote pilote)
        {

        }

        public void CheckTraefik(CiWebServer webserver)
        {

        }

        public CiBuildResult Build(string buildName)
        {
            return null;
            //Pilote.Build(gitRepo, branche, "head", image, tag);
        }

        public void Deploy(string deploymentName)
        {
            //WebServer.Deploy(pilote.repositoryUri, image, tag, uri);
        }
    }
}
