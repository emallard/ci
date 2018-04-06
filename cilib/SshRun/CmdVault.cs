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
    public class CmdVault 
    {
        public void InstallVaultNoTls(ICommandExecute execute)
        {
            var vaultLocalConfig = "{'backend': {'file': {'path': '/vault/file'}}, 'default_lease_ttl': '12h', 'max_lease_ttl': '12h', 'listener': {'tcp': {'address': '127.0.0.1:8200', 'tls_disable': 'true'}}}"
                                .Replace("'", "\"");
            var commandline = "docker run --cap-add=IPC_LOCK -d -e 'VAULT_LOCAL_CONFIG= " + vaultLocalConfig + "' -e 'VAULT_ADDR=http://127.0.0.1:8200' -p 127.0.0.1:8200:8200 --name=dev-vault vault server";
            execute.Command(commandline);
        }

        public void Check(ICommandExecute execute)
        {
            
        }
    }
}