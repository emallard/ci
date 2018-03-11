using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


// When running ciexe inside a container, /cidiata will be in /cidata
// But when debugging, /cidatawill bein /home/test/cidata

public class CiDataDirectory {

    public CiDataDirectory(string path)
    {
        Path = path;
    }

    public string Path { get; }
}