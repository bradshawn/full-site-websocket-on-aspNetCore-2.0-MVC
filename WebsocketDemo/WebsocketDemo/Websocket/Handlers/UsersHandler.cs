using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Encode = System.Text.Encoding;
using WebsocketDemo.Controllers;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace WebsocketDemo.WebSockets.Handlers
{
    public class UsersHandler : MapHandler
    {
        //handler demo
        public static async Task LoginHandler
            (string message, WebSocket webSocket, ArraySegment<Byte> receivedDataBuffer, 
             CancellationToken cancellationToken)
        {
            //get result of servicelogic
            string result = "";
            //convert result string to byte array
            var bytes = Encode.UTF8.GetBytes(result);

            //send to client
            await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
