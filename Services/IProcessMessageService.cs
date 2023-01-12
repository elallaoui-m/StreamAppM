using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.Services
{
    public interface IProcessMessageService
    {
        Task DoWork(CancellationToken stoppingToken);
        void AddSocket(WebSocket socket, TaskCompletionSource<object> _socketFinishedTcs);
    }
}
