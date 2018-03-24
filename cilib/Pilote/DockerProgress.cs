using System.Net;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Docker.DotNet.Models;

namespace cilib
{
    public class DockerProgress : IProgress<JSONMessage> {
        private readonly Action<JSONMessage> _onCalled;

        public DockerProgress(Action<JSONMessage> _onCalled = null)
        {
            this._onCalled = _onCalled;
        }
        void IProgress<JSONMessage>.Report(JSONMessage value)
        {
            if (_onCalled != null)
                _onCalled(value);
        }
    }
}