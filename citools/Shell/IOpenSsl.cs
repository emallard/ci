using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IOpenSsl
    {
        string generateCAKey(string domain);
        string generateCAPem(string caKey, string domain);

        string generateDomainKey(string domain);
        string generateDomainCsr(string domain, string domainKey);
        string generateDomainCrt(string domainCsr, string caPem, string caKey, string domainCrt);
    }
}