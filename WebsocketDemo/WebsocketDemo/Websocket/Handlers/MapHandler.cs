using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebsocketDemo.WebSockets.Handlers
{
    public class MapHandler
    {
        //define the max length of message receive and send
        public const int maxMessageSize = 32768;
        
        //handle message that is sent and mapping to message handler 
        public static async Task Mapping(HttpContext httpContext, WebSocket webSocket)
        {
            var receivedDataBuffer = new ArraySegment<Byte>(new Byte[maxMessageSize]);

            var cancellationToken = new CancellationToken();

            while (webSocket.State == WebSocketState.Open)
            {
                //Reads data.
                WebSocketReceiveResult webSocketReceiveResult =
                    await webSocket.ReceiveAsync(receivedDataBuffer, cancellationToken);
                
                //if message sent is cancel 
                if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                {
                    //close connection
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                        string.Empty, cancellationToken);
                }
                else
                {
                    //get data from submitted if not null
                    byte[] payloadData = receivedDataBuffer.Array.Where(b => b != 0).ToArray();
                    //get string from received byte array
                    string receiveString =
                        System.Text.Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                    //mapping with received string
                    await MappingLocation(receiveString, webSocket, receivedDataBuffer, cancellationToken);
                }
                //clear received data wait for next request
                receivedDataBuffer = new ArraySegment<byte>(new byte[maxMessageSize]);
            }
        }

        public static async Task MappingLocation
            (string originalMsg, WebSocket webSocket, ArraySegment<Byte> receivedDataBuffer, CancellationToken cancellationToken)
        {
            switch (originalMsg.Split("$$$")[0])
            {
                //service logic mapping
                case "login":
                    await UsersHandler.LoginHandler(originalMsg, webSocket, receivedDataBuffer, cancellationToken);
                    break;
            }
        }
    }
}
