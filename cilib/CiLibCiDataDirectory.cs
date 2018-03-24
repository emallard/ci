using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


// When running ciexe inside a container, /cidiata will be in /cidata
// But when debugging, /cidata will bein /home/test/cidata
namespace cilib
{
    public interface ICiLibCiDataDirectory
    {
        string Path { get; }
    }

    public class CiLibCiDataDirectory : ICiLibCiDataDirectory
    {
        public CiLibCiDataDirectory(string path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }

        public string Path { get; }
    }
}