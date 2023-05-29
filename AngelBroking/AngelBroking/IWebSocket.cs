using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelBroking
{
   interface IWebSocket
    {
        bool IsConnected();
        void ConnectforOrderQuote(string feedtype, string jwttoken, string clientcode, string apikey);
        void ConnectforStockQuote(string feedtoken, string clientcode);
        void Send(string Message);
        void Close(bool Abort = false);
        void HeartBeat(string feedtoken, string clientcode);
        void HeartBeat(string feedtype, string jwttoken, string clientcode, string apikey);
        void RunScript(string feedtoken, string clientcode, string script, string task);
    }
}
