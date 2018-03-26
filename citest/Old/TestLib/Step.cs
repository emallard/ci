using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public interface IStep {

    void Test();

    void Run();
    
    void Clean();
}