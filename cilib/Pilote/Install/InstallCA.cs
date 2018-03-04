using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;

public class InstallCA
{
    
    private readonly IConfig config;
    private readonly ShellHelper shellHelper;
    private readonly IInfrastructure infrastructure;

    public InstallCA(
        IConfig config,
        ShellHelper shellHelper,
        IInfrastructure infrastructure)
    {
        this.config = config;
        this.shellHelper = shellHelper;
        this.infrastructure = infrastructure;
    }

    
    public async Task CreateRootCA() 
    {
        var domain = infrastructure.GetVmPilote().PrivateRegistryDomain;
        var ip = infrastructure.GetVmPilote().Ip;

        // CA keys
        shellHelper.Bash($"openssl genrsa -out /ci-data/myCA.key 2048");
        
        shellHelper.Bash(
            "openssl req -x509 -new -nodes -key /ci-data/myCA.key -sha256 -days 1825 -out /ci-data/myCA.pem"
            +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + domain + "' ");
        
        // /ci-data/myCA.pem => must be added as a trust certificate 


        // Domain keys
        shellHelper.Bash($"openssl genrsa -out /ci-data/{domain}.key 2048");
        
        shellHelper.Bash(
            "openssl req -x509 -new -nodes -key /ci-data/{domain}.key -sha256 -days 1825 -out /ci-data/{domain}.csr"
            +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + domain + "' ");
        

        // Sign domain keys with the CA

        // .ext file
        File.WriteAllLines($"/ci-data/{domain}.ext", new string[] {
            "authorityKeyIdentifier=keyid,issuer",
            "basicConstraints=CA:FALSE",
            "keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment",
            "subjectAltName = @alt_names",
            "",
            "[alt_names]",
           $"DNS.1 = {domain}",
           $"DNS.2 = {domain}.{ip}.xip.io"});

        shellHelper.Bash(
            "openssl x509 -req -in /ci-data/{domain}.csr -CA /ci-data/myCA.pem -CAkey /ci-data/myCA.key -CAcreateserial "
            + "-out /ci-data/{domain}.crt -days 1825 -sha256 -extfile /ci-data/{domain}.ext");

        await Task.CompletedTask;
    }


    public async Task CreateRootCA2() 
    {
        var myCAKey = shellHelper.Bash("openssl genrsa 2048");
        
        var myCAPem =  shellHelper.BashAndStdIn(
            "openssl req -x509 -new -nodes -key /dev/stdin -sha256 -days 1825"
            +" -subj '/C=US/ST=NY/L=Somewhere/organizationName=MyOrg/OU=MyDept/CN=" + config.DomainName + "' ", myCAKey);
        

        await Task.CompletedTask;
    }

    private async void StoreCAKeyInVault(string myCAKey)
    {
        //"openssl genrsa -des3 -out myCA.key 2048";

        var vaultAddress = "http://" + config.PiloteIp + ":8200";
        IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("myroot");
        var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);


        var mountpoint = "secret";// + Guid.NewGuid();
        var path = mountpoint + "/CA";
        var values = new Dictionary<string, object>
        {
            {"private.key", myCAKey}
        };

        /*
        await vaultClient.MountSecretBackendAsync(new SecretBackend()
            {
                BackendType = SecretBackendType.Generic,
                MountPoint = mountpoint
            });*/

        await vaultClient.GenericWriteSecretAsync(path, values);

/*
        var readValues = await vaultClient.GenericReadSecretAsync(path);
        var readData = readValues.Data; // gives back the dictionary

        await vaultClient.GenericDeleteSecretAsync(path);
*/
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
        var vaultAddress = "http://127.0.0.1:8200";
        IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("myroot");
        var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);

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