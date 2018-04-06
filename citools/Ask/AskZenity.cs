using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{

    public class AskZenity : IAsk
    {
        private readonly ShellHelper shellHelper;

        public AskZenity(ShellHelper shellHelper)
        {
            this.shellHelper = shellHelper;
        }

        public async Task<string> GetValue(string key)
        {
            await Task.CompletedTask;
            var val = this.shellHelper.Bash("zenity --entry --title='Ask' --text='" + key + "'");
            return val;
        }
    }
}