using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public abstract class IProcessMessage : BackgroundService
    {
        public abstract void AddSocket(WebSocket socket);
    }
}
