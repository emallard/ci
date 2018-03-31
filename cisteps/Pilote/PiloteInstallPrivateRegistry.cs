using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using cilib;

namespace cisteps
{
    public class PiloteInstallPrivateRegistry : IStep 
    {
        public PiloteInstallPrivateRegistry(
            PiloteStep pstep,
            SshCiexe sshPrivateRegistry)
        {
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            await Task.CompletedTask;
            //sshPrivateRegistry.InstallPrivateRegistry(await pstep.GetPiloteSshConnection());
        }


        public async Task TestAlreadyRun()
        {
            await Task.CompletedTask;
        }


        public async Task Check()
        {
            await Task.CompletedTask;
        }
/*
        // HOST
        public void Test()
        {
            var result = vmPilote.SshCommand($"getent hosts {vmPilote.PrivateRegistryDomain}");
            Assert.Contains(vmPilote.PrivateRegistryDomain, result);
        }

        public void Run()
        {
            vmPilote.InstallHosts();
        }

        public void Clean()
        {
            vmPilote.CleanHosts();
        }
         */
    }
}