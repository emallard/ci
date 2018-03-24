using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cisystem
{
    public class CiDeploymentConfig 
    {
        public string Name;
        public string BuildConfig;
        public string WebServer;

        public string PrivateRegistryUrl;
        public string PrivateRegistryUser;
        public string PrivateRegistryPassword;
        
        public string Url;
    }

}