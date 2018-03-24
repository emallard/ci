
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Newtonsoft.Json;

namespace cilib
{
    static class StreamUtil
    {
        public static async Task MonitorStreamAsync(Task<Stream> streamTask, DockerClient client, CancellationToken cancel, IProgress<string> progress)
        {
            using (var stream = await streamTask)
            {
                // ReadLineAsync must be cancelled by closing the whole stream.
                using (cancel.Register(() => stream.Dispose()))
                {
                    using (var reader = new StreamReader(stream, new UTF8Encoding(false)))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            progress.Report(line);
                        }
                    }
                }
            }
        }

        public static async Task MonitorStreamForMessagesAsync<T>(Stream stream, DockerClient client, CancellationToken cancel, IProgress<T> progress)
        {

                // ReadLineAsync must be cancelled by closing the whole stream.
                using (cancel.Register(() => stream.Dispose()))
                {
                    using (var reader = new StreamReader(stream, new UTF8Encoding(false)))
                    {
                        string line;
                        try
                        {
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                var prog = JsonConvert.DeserializeObject<T>(line);
                                //var prog = client.JsonSerializer.DeserializeObject<T>(line);
                                if (prog == null) continue;

                                progress.Report(prog);
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            // The subsequent call to reader.ReadLineAsync() after cancellation
                            // will fail because we disposed the stream. Just ignore here.
                        }
                    }
                }
            
        }
    }
}