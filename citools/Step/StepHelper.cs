using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepHelper
    {
        private readonly IAsk ask;
        private readonly IStore store;
        private IStep step;

        public StepHelper(
            IAsk ask,
            IStore store)
        {
            this.ask = ask;
            this.store = store;
        }

        public StepHelper SetStep(IStep step)
        {
            this.step = step;
            return this;
        }

        Dictionary<string,string> dicAsk = new Dictionary<string, string>();

        private async Task<string> Ask(string key)
        {
            string val;
            if (!dicAsk.TryGetValue(key, out val))
            {
                try { val = await ask.GetValue(key); }
                catch (Exception e) {throw new AskException(step, e);}
                dicAsk[key] = val;
            }
            return val;
        }

        public async Task<string> Need(Uri storeUri, string path)
        {
            try { return await store.Read(storeUri, path); }
            catch (Exception e) {throw new NeedException(step, e);}
        }

        public async Task Keep(Uri storeUri, string key, string path)
        {
            try { await store.Write(storeUri, key, path); }
            catch (Exception e) {throw new KeepException(step, e);}
        }

        public async Task<Uri> AskVaultUriAndToken()
        {
            return new Uri(await Ask("vaultToken") + await Ask("vaultUri"));
        }

        public async Task<SshConnection> NeedSshConnection(Uri storeUri, string vmName)
        {
            var sshUri = new Uri(await Need(storeUri, "admin/" + vmName + "/sshuri"));
            var user = await Need(storeUri, "admin/" + vmName + "/user");
            var password = await Need(storeUri, "admin/" + vmName + "/password");
            return new SshConnection() {
                SshUri = sshUri,
                User = user,
                Password = password,
            };
        }

        public async Task KeepSshConnection(Uri storeUri, string vmName, string sshUri, string user, string password)
        {
            await Keep(storeUri, "admin/" + vmName + "/sshuri", sshUri);
            await Keep(storeUri, "admin/" + vmName + "/user", user);
            await Keep(storeUri, "admin/" + vmName + "/password", password);
        }
    }
}