using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngelBroking;
using Newtonsoft.Json;

namespace AngelBrokingConsoleApp
{
    class StockQuotesSocket
    {
        #region Socket1
        //static void Main(string[] args)
        //{
        //    //Socket connect to get stock prices

        //string Client_code = "";  //YOUR CLIENT CODE
        //string Password = ""; //YOUR PAS SWORD
        //string api_key = "";
        //string JWTToken = "";  // optional
        //string RefreshToken = ""; // optional

        //    SmartApi connect = new SmartApi(api_key, JWTToken, RefreshToken);

        //    OutputBaseClass obj = new OutputBaseClass();

        //    //Login by client code and password
        //    obj = connect.GenerateSession(Client_code, Password);
        //    AngelToken sagr = obj.TokenResponse;

        //    //Get Token
        //    obj = connect.GenerateToken();
        //    sagr = obj.TokenResponse;

        //    WebSocket _WS = new WebSocket();
        //    var exitEvent = new ManualResetEvent(false);

        //    _WS.ConnectforStockQuote(sagr.feedToken, Client_code);
        //    if (_WS.IsConnected())
        //    {
        //        string script = "nse_cm|2885&nse_cm|1594&nse_cm|11536&nse_cm|3045";
        //        //script = "mcx_fo|227536&mcx_fo|220366&mcx_fo|225891&mcx_fo|226858";
        //        _WS.RunScript(sagr.feedToken, Client_code, script, "mw");
        //        _WS.MessageReceived += WriteResult;


        //        //_WS.Close(true);// to stop and close socket connection
        //    }
        //    exitEvent.WaitOne();
        //}
        //static void WriteResult(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine("Tick Received : " + e.Message);

        //}

        #endregion
    }
}
