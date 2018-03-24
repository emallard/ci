

using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cisystem
{
    public class CiSystemConfig {
        
        public List<CiBuildConfig> ListBuildConfig = new List<CiBuildConfig>();
        
        public List<CiDeploymentConfig> ListDeploymentConfig = new List<CiDeploymentConfig>();
    }
}