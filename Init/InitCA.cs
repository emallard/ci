using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;

public class InitCA
{

    public async Task CreateRootCA() 
    {
        var privateKey = ShellHelper.Bash("openssl genrsa 2048");
        //"openssl genrsa -des3 -out myCA.key 2048";

        var vaultAddress = "http://127.0.0.1:8200";
        IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("myroot");
        var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);


        var mountpoint = "secret";// + Guid.NewGuid();
        var path = mountpoint + "/CA";
        var values = new Dictionary<string, object>
        {
            {"private.key", privateKey}
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
}