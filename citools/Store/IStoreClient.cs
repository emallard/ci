using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IStoreClient
    {
        Task<string> ReadSecretAsync(string path);

        Task WriteSecretAsync(string path, string value);

        Task DeleteSecretAsync(string path);
        
        Task<Policy> GetPolicyAsync(string name);

        Task WritePolicyAsync(Policy policy);

        
        Task EnableUsernamePassword();

        Task WriteUser(string user, string password, string policy);
    }
}