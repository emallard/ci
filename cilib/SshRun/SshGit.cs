using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;

namespace cilib
{
    public class SshGit {

        private readonly ISshClient sshClient;
        
        public SshGit(ISshClient sshClient)
        {
            this.sshClient = sshClient;
        }


        public async Task Clean() 
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await dockerWrapper.DeleteContainerIfExistsByName(containerName);
            //await dockerWrapper.DeleteImageIfExists(traefikRepoTag);
        }

    }
}