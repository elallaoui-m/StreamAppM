using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StreamApp.Models;
using StreamApp.Services;
using StreamApp.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamApp.middlewares
{
    public class ManageSendingMessages
    {
        private readonly IConfiguration configuration;
        private readonly ConsumerConfig consumerConfig;
        RequestDelegate next;


        public ManageSendingMessages(IConfiguration configuration,
            ConsumerConfig consumerConfig,
            RequestDelegate _next
            
           )
        {
            this.configuration = configuration;
            this.consumerConfig = consumerConfig;
            next = _next;
        }

        public async Task InvokeAsync(HttpContext context, IProcessMessage ps)
        {
            if (context.Request.Path == "/ws")
            {


                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    ps.AddSocket(socket);
                    await ps.StartAsync(new CancellationToken());
                    TaskCompletionSource<object> _socketFinishedTcs = new TaskCompletionSource<object>();
                    await _socketFinishedTcs.Task;

                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
