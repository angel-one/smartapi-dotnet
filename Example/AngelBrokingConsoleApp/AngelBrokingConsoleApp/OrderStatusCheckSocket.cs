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
    class OrderStatusCheckSocket
    {
        #region Socket2

        //static void Main(string[] args)
        //{

        //    //Socket connect to get order status

        //    string Client_code = "";  //YOUR CLIENT CODE
        //    string Password = ""; //YOUR PAS SWORD
        //    string api_key = "";
        //    string JWTToken = "";  // optional
        //    string RefreshToken = ""; // optional

        //    SmartApi connect = new SmartApi(api_key, JWTToken, RefreshToken);

        //    OutputBaseClass obj = new OutputBaseClass();

        //    //Login by client code and password
        //    obj = connect.GenerateSession(Client_code, Password);
        //    AngelToken sagr = obj.TokenResponse;

        //    //Get Token
        //    obj = connect.GenerateToken();
        //    sagr = obj.TokenResponse;

        //    WebSocket _WS2 = new WebSocket();
        //    var exitEvent = new ManualResetEvent(false);

        //    //Place Order
        //    OrderInfo sord = new OrderInfo();
        //    sord.variety = Constants.VARIETY_NORMAL;
        //    sord.tradingsymbol = "SBIN-EQ";
        //    sord.symboltoken = "3045";
        //    sord.transactiontype = Constants.TRANSACTION_TYPE_BUY;
        //    sord.exchange = Constants.EXCHANGE_NSE;
        //    sord.ordertype = Constants.ORDER_TYPE_LIMIT;
        //    sord.producttype = Constants.PRODUCT_TYPE_INTRADAY;
        //    sord.duration = Constants.VALIDITY_DAY.ToString();
        //    sord.price = "19500";
        //    sord.squareoff = "0";
        //    sord.stoploss = "0";
        //    sord.quantity = "1";

        //    obj = connect.placeOrder(sord);
        //    OrderResponse sOres = obj.PlaceOrderResponse;

        //    //get Order Book
        //    obj = connect.getOrderBook();
        //    GetOrderBookResponse book = obj.GetOrderBookResponse;

        //    Console.WriteLine("------getOrderBook call output-------------");
        //    Console.WriteLine(JsonConvert.SerializeObject(book));
        //    Console.WriteLine("----------------------------------------------");

        //    _WS2.ConnectforOrderQuote(sagr.feedToken, sagr.jwtToken, Client_code, api_key);
        //    if (_WS2.IsConnected())
        //    {
        //        _WS2.MessageReceived += WriteResult;

        //        string feedtype = "order_feed";
        //        string actiontype = "subscribe";
        //        _WS2.FetchOrderQuotes(sagr.jwtToken, Client_code, api_key, actiontype, feedtype);

        //        //_WS2.Close();
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
