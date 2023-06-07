using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AngelBroking;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Threading;


namespace Websocket2
{
    class PlaceOrderApi
    {
        static Ticker ticker;
        static string clientCode = "V51320342";
        static string password = "0105";
        static string apiKey = "Qkgu7The";
        static string jwtToken = "";  // optional
        static string refreshToken = ""; // optional
        static string totp = "068055";

      
        public static TickerRequest initialRequest = new TickerRequest
        {
            correlationID = "abcde12345",
            action = 1,
            param = new TokenParams
            {
                mode = 1,
                tokenList = new List<TokenList>
        {
            new TokenList
            {
                exchangeType = 1,
                tokens = new List<string>
                {
                    "1594"
                }
            }
          ,
            new TokenList
              {
                exchangeType = 7, // Add the desired exchangeType for the new TokenList
                tokens = new List<string>
                {
                    "JEERAUNJHA20JUN2023"
                }
               }
              }
            }
        };

        static void Main(string[] args)
        {

            SmartApi connect = new SmartApi(apiKey, jwtToken, refreshToken);

            OutputBaseClass obj = new OutputBaseClass();

            // Login by client code and password
            obj = connect.GenerateSession(clientCode, password, totp);
            AngelToken sagr = obj.TokenResponse;

            Console.WriteLine("------GenerateSession call output-------------");
            Console.WriteLine(JsonConvert.SerializeObject(sagr));
            Console.WriteLine("----------------------------------------------");

            // Initialize WebSocket ticker
            initTicker(sagr.jwtToken, apiKey, clientCode, sagr.feedToken);

            Console.ReadLine();

        }

        private static void initTicker(string jwttoken, string api_key, string client_code, string feedtoken)
        {
            ticker = new Ticker(jwttoken, api_key, client_code, feedtoken);

            ticker.OnTickLtp += OnTickLtp;
            ticker.OnTickQuote += OnTickQuote;
            ticker.OnTick += OnTick;
            ticker.OnTickPong += OnTickPong;
            ticker.OnError += OnError;
            ticker.OnClose += OnClose;
            ticker.OnConnect += OnConnect;


            ticker.Connect();

            string initialMsg = Utils.JsonSerialize(initialRequest);
            ticker.SetMode(initialMsg);

        }

        private static void OnConnect()
        {
            Console.WriteLine("Connected ticker");
            ticker.InitiatePingTimer();

        }

        private static void OnClose()
        {
            Console.WriteLine("Closed ticker");
        }

        private static void OnError(string Message)
        {
            Console.WriteLine("Error: " + Message);

            Reconnect();
           
        }

        private static void Reconnect()
        {
            // Close the ticker WebSocket connection
            ticker.CloseForSocket();

            // Delay before reconnecting (e.g., 5 seconds)
            Thread.Sleep(2000);

            // Reconnect and resubscribe
            ticker.Connect();

            string initialMsg = Utils.JsonSerialize(initialRequest);
            ticker.SetMode(initialMsg);
        }

        private static void UnSubscribe()
        {
            initialRequest.action = 0; // Update the action to 0
            string unsubscribe = Utils.JsonSerialize(initialRequest);
            ticker.SetMode(unsubscribe);
        }
        private static void OnTickLtp(TickLtp TickData)
        {
            Console.WriteLine("TickLTP " + Utils.JsonSerialize(TickData));

              //UnSubscribe();
        }

        private static void OnTickQuote(TickQuote TickData)
        {
            Console.WriteLine("TickQuote " + Utils.JsonSerialize(TickData));

            //UnSubscribe();
        }

        private static void OnTick(Tick TickData)
        {
            Console.WriteLine("Tick " + Utils.JsonSerialize(TickData));

            //UnSubscribe();
        }

      
        private static void OnTickPong(TickPong TickData)
        {
            // Cancel the ping timer
            ticker.pingTimer?.Dispose();

            // Reset the ping timer for subsequent pings
            ticker.ResetPingTimer();

            // Handle the pong response as usual
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string message = "Ping " + Utils.JsonSerialize(TickData);
            Console.WriteLine(message + " " + timestamp);
        }


    }
}