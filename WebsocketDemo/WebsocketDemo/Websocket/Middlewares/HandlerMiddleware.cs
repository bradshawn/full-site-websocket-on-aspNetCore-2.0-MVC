using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebsocketDemo.WebSockets.Handlers;

// ReSharper disable once CheckNamespace
namespace WebsocketDemo.WebSockets.Handlers
{
    public class HandlerMiddleware
    {
        private readonly RequestDelegate next;

        public HandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                //Handle data by javascript submitted
                await MapHandler.Mapping(context, webSocket);
            }
            
            await this.next(context);
        }
    }

    public static class WebSocketRequestHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketRequestHandlerMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandlerMiddleware>();
        }
    }
}
