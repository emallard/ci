using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class OpenSslShell : IOpenSsl
    {
        private readonly ShellHelper shellHelper;

        public OpensslShell(ShellHelper shellHelper)
        {
            this.shellHelper = shellHelper;
        }

        public string generateCAKey(string domain)
        {
            throw new NotImplementedException();
        }

        public string generateCAPem(string caKey, string domain)
        {
            throw new NotImplementedException();
        }

        public string generateDomainCrt(string domainCsr, string caPem, string caKey, string domainCrt)
        {
            throw new NotImplementedException();
        }

        public string generateDomainCsr(string domain, string domainKey)
        {
            throw new NotImplementedException();
        }

        public string generateDomainKey(string domain)
        {
            throw new NotImplementedException();
        }
    }
}