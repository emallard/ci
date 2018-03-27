using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using citools;

namespace cilib
{
    public class InstallCA
    {
        
        private readonly ShellHelper shellHelper;
        private readonly ICiLibCiDataDirectory cidataDir;

        public InstallCA(
            ShellHelper shellHelper,
            ICiLibCiDataDirectory cidataDir)
        {
            this.shellHelper = shellHelper;
            this.cidataDir = cidataDir;
        }

        
        public Task Install()
        {
            throw new NotImplementedException();
        }

        public async Task Install(string privateRegistryDomain)
        {
            var domain = privateRegistryDomain;
            //var ip = infrastructure.GetVmPilote().Ip;

            var dir = cidataDir + "/tls";
            shellHelper.Bash($"mkdir -p {dir}");

            // CA keys
            shellHelper.Bash($"openssl genrsa -out {dir}/myCA.key 2048");
            
            shellHelper.Bash(
                $"openssl req -x509 -new -nodes -key {dir}/myCA.key -sha256 -days 1825 -out {dir}/myCA.pem"
                +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + domain + "' ");
            
            // {dir}/myCA.pem => must be added as a trust certificate 


            // Domain keys
            shellHelper.Bash($"openssl genrsa -out {dir}/{domain}.key 2048");
            
            shellHelper.Bash(
                $"openssl req -new -key {dir}/{domain}.key -sha256 -days 1825 -out {dir}/{domain}.csr"
                +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + domain + "' ");
            

            // Sign domain keys with the CA

            // .ext file
            File.WriteAllLines($"{dir}/{domain}.ext", new string[] {
                "authorityKeyIdentifier=keyid,issuer",
                "basicConstraints=CA:FALSE",
                "keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment",
                "subjectAltName = @alt_names",
                "",
                "[alt_names]",
            $"DNS.1 = {domain}"});

            shellHelper.Bash(
                $"openssl x509 -req -in {dir}/{domain}.csr -CA {dir}/myCA.pem -CAkey {dir}/myCA.key -CAcreateserial "
                + $"-out {dir}/{domain}.crt -days 1825 -sha256 -extfile {dir}/{domain}.ext");


            // Copy keys to the privateregistry folder
            shellHelper.Bash($"rm -rf {cidataDir}/privateregistry/certs");
            shellHelper.Bash($"mkdir -p {cidataDir}/privateregistry/certs");
            shellHelper.Bash($"cp {dir}/{domain}.* {cidataDir}/privateregistry/certs");

            await Task.CompletedTask;
        }

        public async Task Clean() 
        {
            var dir = cidataDir + "/tls";
            shellHelper.Bash($"rm -rf {dir}");

            await Task.CompletedTask;
        }


        public async Task CreateRootCA2(string infrastructureDomainName) 
        {
            var myCAKey = shellHelper.Bash("openssl genrsa 2048");
            
            var myCAPem =  shellHelper.BashAndStdIn(
                "openssl req -x509 -new -nodes -key /dev/stdin -sha256 -days 1825"
                +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + infrastructureDomainName + "' ", myCAKey);
            

            await Task.CompletedTask;
        }



        public async Task InitNewHost(string host)
        {
            /* 
            openssl genrsa -out host+".key" 2048

            Then we create a CSR:

            openssl req -new -key host+".key" -out host+".csr"

            openssl x509 -req -in dev.mergebot.com.csr -CA myCA.pem -CAkey myCA.key -CAcreateserial \
            -out dev.mergebot.com.crt -days 1825 -sha256 -extfile dev.mergebot.com.ext
            */

    /*
            I now have three files: 
            dev.mergebot.com.key (the private key), 
            dev.mergebot.com.csr (the certificate signing request), 
            and dev.mergebot.com.crt (the signed certificate).
    */
    /*
            var vaultAddress = "http://127.0.0.1:8200";
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("myroot");
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);
    */
    /*
            var mountpoint = "secret";// + Guid.NewGuid();
            var path = mountpoint + "/" + host;
            var values = new Dictionary<string, object>
            {
                {"private.key", hostCAKey}
            };
    */
            await Task.CompletedTask;
        }
    }
}