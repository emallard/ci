using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class OpenSslMock : IOpenSsl
    {
        private readonly ShellHelper shellHelper;

        public OpenSslMock()
        {
            this.shellHelper = shellHelper;
        }

        public string generateCAKey(string domain)
        {
            return "generateCAKey"+domain;
        }

        public string generateCAPem(string caKey, string domain)
        {
            return "generateCAPem"+domain;
        }

        public string generateDomainCrt(string domainCsr, string caPem, string caKey, string domainCrt)
        {
            return "generateCrt"+domainCrt;
        }

        public string generateDomainCsr(string domain, string domainKey)
        {
            return "generateCsr"+domain;
        }

        public string generateDomainKey(string domain)
        {
            return "generateKey"+domain;
        }
    }
}