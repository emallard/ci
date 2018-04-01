using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using citools;

namespace cisteps
{
    public class DevOpInitSsl : IStep
    {
        private readonly ListAsk listAsk;
        private readonly ListResources listResources;
        private readonly Installer installer;
        private readonly IOpenSsl openSsl;

        public DevOpInitSsl(
            ListAsk listAsk,
            ListResources listResources,
            Installer installer,
            IOpenSsl openSsl
        )
        {
            this.listAsk = listAsk;
            this.listResources = listResources;
            this.installer = installer;
            this.openSsl = openSsl;
        }

        public async Task Check()
        {
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());

            StepAssert.IsTrue(null != await listResources.CAKey.Read(auth));
            StepAssert.IsTrue(null != await listResources.CAPem.Read(auth));
        }

        public async Task Run()
        {   
            installer.Install("OpenSsl");

            var caDomain = await listAsk.CADomain.Ask();
            var caKey = this.openSsl.generateCAKey(caDomain);
            var caPem = this.openSsl.generateCAPem(caKey, caDomain);

            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await listAsk.LocalVaultDevopUser.Ask(),
                await listAsk.LocalVaultDevopPassword.Ask());

            await listResources.CAKey.Write(auth, caDomain);
            await listResources.CAPem.Write(auth, caPem);
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }
    }
}
