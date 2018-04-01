using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AskResourceLogDto
    {
        public string Name {get; set;}
        
        public AskResourceLogDto()
        {
        }

        public AskResourceLogDto(AskResource askResource)
        {
            this.Name = askResource.Name();
        }
    }
}