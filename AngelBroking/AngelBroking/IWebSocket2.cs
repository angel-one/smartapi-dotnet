using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBroking
{
    public interface IWebSocket2
    {
        #region WebSocketv2

        event OnConnectHandler OnConnect;
        event OnCloseHandler OnClose;
        event OnDataHandler OnData;
        event OnErrorHandler OnError;
        void Connect(string Url, Dictionary<string, string> headers = null);
        Task SendAsync(string Message);
        Task ReceiveAsync();
        bool IsConnectedForSocket();
        void CloseForSocket();


        void Heartbeat(string Message);



        #endregion
    }
}
