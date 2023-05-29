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
    public class WebSocket : IWebSocket
    {
        
        ManualResetEvent receivedEvent = new ManualResetEvent(false);
        int receivedCount = 0;
        WebsocketClient _ws;        
        string _url = "wss://smartapisocket.angelbroking.com/websocket?";
        string _url2 = "wss://omnefeeds.angelbroking.com/NestHtml5Mobile/socket/stream";

        public event EventHandler<MessageEventArgs> MessageReceived;
        
        public WebSocket()
        {

        }
        public bool IsConnected()
        {
            if (_ws is null)
                return false;

            return _ws.IsStarted;
        }
        public void ConnectforOrderQuote(string feedtype, string jwttoken, string clientcode, string apikey)
        {
            try
            {
                //var receivedEvent = new ManualResetEvent(false);

                string finalurl = _url + "jwttoken=" + jwttoken + "&clientcode=" + clientcode + "&apikey=" + apikey;
                var url = new Uri(finalurl);

                _ws = new WebsocketClient(url);

                _ws.MessageReceived.Subscribe(msg => Receive(msg.Text));
               
                _ws.Start();
                int i = 0;
                do
                {
                    HeartBeat(feedtype, jwttoken, clientcode, apikey);
                    Thread.Sleep(60);
                    i++;
                } while (i < 10);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Receive(string Message)
        {
            MessageEventArgs args = new MessageEventArgs();
            //args.Message = Helpers.DecodeBase64(Message);
            args.Message = Message;
            EventHandler<MessageEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler(this, args);
            }
            receivedCount++;
            if (receivedCount >= 10)
                receivedEvent.Set();
        }
        public void ConnectforStockQuote(string feedtoken, string clientcode)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                if (feedtoken != "" && clientcode != "")
                {
                    string ConnectionMsg = "{\"task\":\"cn\",\"channel\":\"\",\"token\":\"" + feedtoken + "\",\"user\": \"" + clientcode + "\",\"acctid\":\"" + clientcode + "\"}";
                    var url = new Uri(_url2);
                    _ws = new WebsocketClient(url);
                    _ws.MessageReceived.Subscribe(msg => Receive2(msg.Text));
                    _ws.Start();
                    _ws.Send(ConnectionMsg);
                    int i = 0;
                    do
                    {
                        HeartBeat(feedtoken, clientcode);
                        Thread.Sleep(60);
                        i++;
                    } while (i<10);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RunScript(string feedtoken, string clientcode, string script, string task)
        {
            string strwatchlistscrips = "";
            if (feedtoken != "" && clientcode != "")
            {
                if (_ws.IsStarted)
                {
                    try
                    {
                        if (task != null && task != "")
                        {
                            if (task == "mw" || task == "sfi" || task == "dp")
                            {
                                strwatchlistscrips = script;   //"nse_cm|2885&nse_cm|1594&nse_cm|11536";
                                string scriptReq = "{\"task\":\"mw\",\"channel\":\"" + strwatchlistscrips + "\",\"token\":\"" + feedtoken + "\",\"user\": \"" + clientcode + "\",\"acctid\":\"" + clientcode + "\"}";
                                _ws.Send(scriptReq);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        public void Receive2(string Message)
        {
            MessageEventArgs args = new MessageEventArgs();
            try
            {
                byte[] messagebytes = Helpers.DecodeBase64Byte(Message);
                using (MemoryStream ms = new MemoryStream(messagebytes))
                {
                    using (var output = new MemoryStream())
                    using (var zipStream = new zlib.ZOutputStream(output))
                    {
                        using (ms)
                        {
                            var buffer = new byte[2000];
                            int len;

                            while ((len = ms.Read(buffer, 0, 2000)) > 0)
                            {
                                zipStream.Write(buffer, 0, len);
                            }
                        }
                        // reset output stream to start so we can read it to a string
                        output.Position = 0;

                        byte[] content = new byte[output.Length];

                        output.Read(content, 0, (int)output.Length);

                        args.Message = Encoding.Default.GetString(content);
                    }
                }
                receivedCount++;
                if (receivedCount >= 10)
                    receivedEvent.Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            EventHandler<MessageEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        public void HeartBeat(string feedtoken, string clientcode)
        {
            string hbmsg = "{\"task\":\"hb\",\"channel\":\"\",\"token\":\"" + feedtoken + "\",\"user\": \"" + clientcode + "\",\"acctid\":\"" + clientcode + "\"}";
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(hbmsg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Send(string Message)
        {
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Close(bool Abort = false)
        {
            if (_ws.IsRunning)
            {
                if (Abort)
                    _ws.Stop(WebSocketCloseStatus.NormalClosure, "Close");
                else
                {
                    _ws.Dispose();
                }
            }
        }
        public void FetchOrderQuotes(string jwttoken,string clientcode,string apikey, string actiontype, string feedtype)
        {
            if (actiontype == "subscribe" || actiontype == "unsubscribe")
            {
                // var feedtype = feed_type;   //"nse_cm|2885&nse_cm|1594&nse_cm|11536"; //order_feed
                var _req = "{\"actiontype\":\"" + actiontype + "\",\"feedtype\":\"" + feedtype + "\",\"jwttoken\":\"" + jwttoken + "\",\"clientcode\": \"" + clientcode + "\", \"apikey\":\"" + apikey + "\"}";
                if (_ws.IsStarted)
                {
                    try
                    {
                        _ws.Send(_req);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        public void HeartBeat(string feedtype, string jwttoken, string clientcode, string apikey)
        {
            string hbmsg = "{\"actiontype\":\"heartbeat\",\"feedtype\":\"" + feedtype + "\",\"jwttoken\":\"" + jwttoken + "\",\"clientcode\": \"" + clientcode + "\", \"apikey\":\"" + apikey + "\"}";
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(hbmsg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
