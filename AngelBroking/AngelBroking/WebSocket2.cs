using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Net;
using Websocket.Client;
using System.IO;
using zlib;

namespace AngelBroking
{
    #region WebSocketv2
    // Delegates for Events
    public delegate void OnConnectHandler();
    public delegate void OnCloseHandler();
    public delegate void OnErrorHandler(string Message);
    public delegate void OnDataHandler(byte[] Data, bool EndOfMessage, string MessageType);

    #endregion
    public class WebSocket2 : IWebSocket2
    {
        #region WebSocketv2

        ClientWebSocket _cws;
        string _curl;
    
        private SemaphoreSlim _lockObject = new SemaphoreSlim(1, 1);


        public event OnConnectHandler OnConnect;
        public event OnCloseHandler OnClose;
        public event OnDataHandler OnData;
        public event OnErrorHandler OnError;

        private bool _isWebSocketAborted = false;

        public WebSocket2()
        {

        }
        public bool IsConnectedForSocket()
        {
            if (_cws is null)
                return false;

            return _cws.State == WebSocketState.Open;
        }
       

        public void Connect(string Url, Dictionary<string, string> headers = null)
        {
            _curl = Url;
            try
            {
                // Initialize ClientWebSocket instance and connect with Url
                _cws = new ClientWebSocket();
                if (headers != null)
                {
                    foreach (string key in headers.Keys)
                    {
                        _cws.Options.SetRequestHeader(key, headers[key]);
                    }
                }

                _cws.ConnectAsync(new Uri(_curl), CancellationToken.None).Wait();

                OnConnect?.Invoke();
            }
            catch (Exception e)
            {
                if (_cws.State == WebSocketState.Aborted)
                {
                    _isWebSocketAborted = true;
                }
                OnError?.Invoke("Error while receiving data. Message: " + e.Message);
            }
        }


        public async Task SendAsync(string Message)
        {
            if (_cws.State == WebSocketState.Open && !_isWebSocketAborted)
            {
                try
                {
                    _cws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(Message)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while sending data. Message: " + e.Message);
                }
            }
        }


        public async Task ReceiveAsync()
        {
            while (_cws.State == WebSocketState.Open && !_isWebSocketAborted)
            {
                try
                {
                    var buffer = new byte[1024];
                    WebSocketReceiveResult result;

                    // Use a lock to synchronize access to the WebSocket instance
                    await _lockObject.WaitAsync();
                    try
                    {
                        result = await _cws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }
                    finally
                    {
                        _lockObject.Release();
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _cws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        OnClose?.Invoke();
                        break;
                    }
                    else
                    {
                        OnData?.Invoke(buffer, result.EndOfMessage, result.MessageType.ToString());
                    }
                }
                catch (Exception e)
                {
                    if (_cws.State == WebSocketState.Aborted)
                    {
                        _isWebSocketAborted = true;
                        OnError?.Invoke("WebSocket connection aborted.");
                        break;
                    }
                    else
                    {
                        OnError?.Invoke("Error while receiving data. Message: " + e.Message);
                    }
                }
            }
        }



        public void CloseForSocket(bool Abort = false)
        {
            if (_cws.State == WebSocketState.Open)
            {
                try
                {
                    if (_isWebSocketAborted)
                    {
                        Console.WriteLine("WebSocket connection aborted.");
                    }
                    else
                    {
                        if (Abort)
                        {
                            _cws.Abort();
                            Console.WriteLine("Aborted successfully");
                        }
                        else
                        {
                            _cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
                           
                        }
                    }
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while closing connection. Message: " + e.Message);
                }
            }
        }


        public void Heartbeat(string Message)
        {
            if (_cws.State == WebSocketState.Open)
            {
                try
                {
                    SendAsync(Message).Wait();
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while sending heartbeat message. Message: " + e.Message);
                }
            }
        }






        #endregion
    }

}