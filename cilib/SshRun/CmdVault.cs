using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;
using citools;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace cilib
{
    public class CmdVault 
    {
        public void InstallVaultNoTls(ICommandExecute execute, IVaultSealKeys vaultSealKeys)
        {
            var vaultLocalConfig = "{\"backend\":{\"file\":{\"path\":\"/vault/file\"}}, \"listener\":{\"tcp\":{\"address\":\"0.0.0.0:8200\", \"tls_disable\":\"1\"}}}";
            
            var env = 
                " -e 'VAULT_LOCAL_CONFIG=" + vaultLocalConfig + "'"
              + " -e 'VAULT_ADDR=http://127.0.0.1:8200'"
              + " -e 'VAULT_REDIRECT_ADDR=http://127.0.0.1:8201' ";

            var volumes = "";

            //var commandline = "docker run --cap-add=IPC_LOCK -d " + env + " -p 127.0.0.1:8200:8200 --name=dev-vault vault server";
            var commandline = "docker run --cap-add=IPC_LOCK -d " + env + volumes + " -p 8200:8200 --name=dev-vault vault server";
            Console.WriteLine(commandline);
            execute.Command(commandline);
            
            Thread.Sleep(3000);

            var initResponse = execute.Command("curl --request PUT --data '{\"secret_shares\":5, \"secret_threshold\":3}' http://localhost:8200/v1/sys/init");
           
            dynamic initResult = JObject.Parse(initResponse);
            //var lines = initResult.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
            
            //var sealkeysLines = lines.Where(l => l.StartsWith("Unseal Key")).ToList();
            //var sealkeys = sealkeysLines.Select(s => s.Substring("Unseal Key 1: ".Length)).ToList();

            var sealkeys = new List<string>();
            for (var i=0; i<5; ++i)
                sealkeys.Add((string) initResult.keys[i]);
            vaultSealKeys.SetSealKeys(sealkeys);

            //var rootTokenLine = lines.First(l => l.StartsWith("Initial Root Token: "));
            //var rootToken = rootTokenLine.Substring("Initial Root Token: ".Length);

            var rootToken = (string) initResult.root_token;
            vaultSealKeys.SetRootToken(rootToken);

            string unseal = "";
            for (var i=0; i<3; ++i)
            {
                //var cmd = "docker exec dev-vault vault operator unseal " + sealkeys[i];
                var key = sealkeys[i];
                unseal = execute.Command("curl --request PUT --data '{\"key\": \""+ key +"\"}' http://localhost:8200/v1/sys/unseal");
            }

            var header = " --header \"X-Vault-Token: " + rootToken + "\" ";


            // create a test user password
            var data = " --data '{\"type\": \"userpass\", \"description\": \"Login with user password\"}' ";
            var enableAuthCmd = "curl " + header + " --request PUT " + data + " http://127.0.0.1:8200/v1/sys/auth/userpass ";
            
            Console.WriteLine(enableAuthCmd);
            var enableAuthResponse = execute.Command(enableAuthCmd);

            
            var capabilities = "capabilities = [\\\"create\\\", \\\"read\\\", \\\"update\\\", \\\"delete\\\", \\\"list\\\"]";
            data = " --data '{\"policy\": \"path \\\"secret/test\\\" { " + capabilities + " }\"}' ";
            var policyCmd = "curl " + header + " --request PUT " + data + " http://127.0.0.1:8200/v1/sys/policy/test-policy ";

            Console.WriteLine(policyCmd);
            var policyResponse = execute.Command(policyCmd);

            data = " --data '{\"password\": \"test\", \"policies\": \"test-policy\" }' ";
            var userCmd = "curl " + header + " --request POST " + data + " http://127.0.0.1:8200/v1/auth/userpass/users/test ";

            Console.WriteLine(userCmd);
            execute.Command(userCmd);
        }

        public void CleanVaultNoTls(ICommandExecute execute)
        {
            var commandline = "docker rm -f dev-vault";
            execute.Command(commandline);
            Thread.Sleep(3000);
        }
        
        public void Check(ICommandExecute execute)
        {
            
            StepAssert.Contains("\"initialized\":true,\"sealed\":false" , execute.Command("curl http://localhost:8200/v1/sys/health"));

            // login for test user
            var data = " --data '{\"password\": \"test\"}' ";
            var loginCmd = "curl --request POST " + data + " http://127.0.0.1:8200/v1/auth/userpass/login/test ";

            dynamic loginResponse = JObject.Parse(execute.Command(loginCmd));
            var token = (string) loginResponse.auth.client_token;
            StepAssert.IsTrue(!string.IsNullOrWhiteSpace(token));

            /*
            // Try to read the test value with the test user/password
            var header = " --header \"X-Vault-Token: " + token + "\" ";
            var readCmd = "curl " + token + " http://127.0.0.1:8200/v1/secret/test";
            var readCmdResponse = execute.Command(readCmd);
            StepAssert.AreEqual("testValue", readCmdResponse);
            */
        }
    }
}