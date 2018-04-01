using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class InstallerLogDto
    {
        public string Name { get; set; }

        public InstallerLogDto(){}

        public InstallerLogDto(string name)
        {
            Name = name;
        }
    }
}