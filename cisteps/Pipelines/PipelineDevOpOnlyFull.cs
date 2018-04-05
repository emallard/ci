using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineDevOpOnlyFull : IPipeline
    {
        private readonly ListAsk listAsk;
        private readonly ListResources listResources;
        Func<Task> run;

        public PipelineDevOpOnlyFull(
            IStepRunner runner,
            ListAsk listAsk,
            ListResources listResources,
            InstallDockerSsh installDocker,
            PipelineDevopInit pipelineDevOpInit,
            InstallTraefikSsh installTraefikSsh,
            AddGitToBuild addGitToBuild,
            DockerBuildSsh dockerBuildSsh
            )
        {
            installDocker.SetSshConnectionFunc(this.GetDevOpSshConnection);
            installTraefikSsh.SetSshConnectionFunc(this.GetDevOpSshConnection);

            this.run = async () => {
                await pipelineDevOpInit.Run();
                await runner.Run(installDocker);
                await runner.Run(installTraefikSsh);
                await runner.Run(addGitToBuild);
                await runner.Run(dockerBuildSsh);
            };
            this.listAsk = listAsk;
            this.listResources = listResources;
        }
        
        public async Task Run()
        {
            await this.run();
        }

        private async Task<SshConnection> GetDevOpSshConnection()
        {
            //IAuthenticationInfo auth = new TokenAuthenticationInfo(await listAsk.LocalVaultToken.Ask());

            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());
            return await listResources.WebServerSshConnection.Read(auth);
        }
    }
}