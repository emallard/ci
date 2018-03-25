using System;

namespace cicli
{
    class Program
    {
        static void Main(string[] args)
        {
            var help = @"
            - ci init

            // ci infra needs {devop-infra token} to be executed
            - ci infra pilote create
            - ci infra pilote install-docker

            // ci admin needs {devop-admin token} to be executed
            - ci admin pilote build-ci
            - ci admin pilote ci install-vault

            - ci admin pilote ci install-ca
            - ci admin pilote ci install-private-registry
            - ci admin webserver ci install-traefik

            - ci admin pilote ci addbuildconf {name} {content}
            - ci admin pilote ci adddeployconf {name} {content}

            - ci admin pilote ci build {name}
            - ci admin webserver ci deploy {name}";
            
        }
    }
}
