using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class Installer
    {
        private readonly ILogger logger;

        public Installer(ILogger logger)
        {
            this.logger = logger;
        }

        public void Install(string name)
        {
            logger.Log(new InstallerLogDto(name));
        }
    }
}