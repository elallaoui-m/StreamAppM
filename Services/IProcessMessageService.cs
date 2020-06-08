using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    interface IProcessMessageService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
