using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace AngelBroking
{
    #region WebSocketv2

    public class Ticker
    {
        private string root = Constants.ROOT;
        private string apiKey;
        private string accessToken;
        private string socketUrl = "";
        private string clientCode;
        private string feedToken;

        System.Timers.Timer timer;

        private bool isTimerRunning = false;

        public Timer pingTimer;

        private IWebSocket2 _cws;

        public delegate void OnConnectHandler();

        public delegate void OnCloseHandler();

        public delegate void OnTickHandler(Tick TickData);

        public delegate void OnTickLtpHandler(TickLtp TickData);

        public delegate void OnTickQuoteHandler(TickQuote TickData);

        public delegate void OnTickPongHandler(TickPong TickData);

        public delegate void OnErrorHandler(string Message);

        public event OnConnectHandler OnConnect;

        public event OnCloseHandler OnClose;

        public event OnTickHandler OnTick;

        public event OnTickLtpHandler OnTickLtp;

        public event OnTickQuoteHandler OnTickQuote;

        public event OnTickPongHandler OnTickPong;

        public event OnErrorHandler OnError;


        public Ticker(string jwttoken, string api_key, string client_code, string feed_token, string Root = null, IWebSocket2 CustomWebSocket = null)
        {
            apiKey = api_key;
            accessToken = jwttoken;
            clientCode = client_code;
            feedToken = feed_token;

            if (!String.IsNullOrEmpty(Root))
                root = Root;
            socketUrl = root;
            // initialize websocket
            if (CustomWebSocket != null)
            {
                _cws = CustomWebSocket;
            }
            else
            {
                _cws = new WebSocket2();
            }

            _cws.OnConnect += onConnect;
            _cws.OnData += OnData;
            _cws.OnClose += onClose;
            _cws.OnError += onError;
        }

        private void onError(string Message)
        {
            // pipe the error message from ticker to the events
            OnError?.Invoke(Message);
        }

        private void onClose()
        {
            if (isTimerRunning)
            {
                timer.Stop();
                isTimerRunning = false;
            }

            OnClose?.Invoke();
        }


        /// <summary>
        /// 
        /// Reads an ltp mode tick from raw binary data
        /// </summary>
        private TickLtp ReadLTP(byte[] response)
        {
            TickLtp tickltp = new TickLtp();
            tickltp.mode = Constants.MODE_LTP;
            int SubscriptionMode = response[0];
            tickltp.subscription_mode = Convert.ToUInt16(SubscriptionMode);
            int ExchangeType = response[1];
            tickltp.exchange_type = Convert.ToUInt16(ExchangeType);
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tickltp.token = parts[0];
            tickltp.sequence_number = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            //DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var epocSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tickltp.ExchangeTimestam = epocSeconds;
            var ltp = BitConverter.ToInt32(response.Skip(43).Take(8).ToArray(), 0);
            tickltp.last_traded_price = ltp;
            return tickltp;
        }

        /// <summary>
        /// Reads a quote mode tick from raw binary data
        /// </summary>
        private TickQuote ReadQuote(byte[] response)
        {
            TickQuote tickquote = new TickQuote();
            tickquote.mode = Constants.MODE_QUOTE;
            int SubscriptionMode = response[0];
            tickquote.subscription_mode = Convert.ToUInt16(SubscriptionMode);
            int exchangeType = response[1];
            tickquote.exchange_type = response[1];
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tickquote.token = parts[0];
            tickquote.sequence_number = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            var exchangeTimeStampInMilliSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tickquote.ExchangeTimestam = exchangeTimeStampInMilliSeconds;
            var ltp = BitConverter.ToInt64(response.Skip(43).Take(8).ToArray(), 0);
            tickquote.last_traded_price = ltp;
            tickquote.last_traded_quantity = BitConverter.ToInt64(response.Skip(51).Take(8).ToArray(), 0);
            var averageTradedPrice = BitConverter.ToInt64(response.Skip(59).Take(8).ToArray(), 0);
            tickquote.avg_traded_price = averageTradedPrice;
            tickquote.vol_traded = BitConverter.ToInt64(response.Skip(67).Take(8).ToArray(), 0);
            tickquote.total_buy_quantity = BitConverter.ToDouble(response.Skip(75).Take(8).ToArray(), 0);
            tickquote.total_sell_quantity = BitConverter.ToDouble(response.Skip(83).Take(8).ToArray(), 0);
            var openPriceOfTheDay = BitConverter.ToInt64(response.Skip(91).Take(8).ToArray(), 0);
            tickquote.open_price_day = openPriceOfTheDay;
            var highPriceOfTheDay = BitConverter.ToInt64(response.Skip(99).Take(8).ToArray(), 0);
            tickquote.high_price_day = highPriceOfTheDay;
            var lowPriceOfTheDay = BitConverter.ToInt64(response.Skip(107).Take(8).ToArray(), 0);
            tickquote.low_price_day = lowPriceOfTheDay;
            var closePrice = BitConverter.ToInt64(response.Skip(115).Take(8).ToArray(), 0);
            tickquote.close_price = closePrice;
            return tickquote;

        }

        /// <summary>
        /// Reads a snapquote mode tick from raw binary data
        /// </summary>
        private Tick ReadFull(byte[] response)
        {
            Tick tick = new Tick();
            tick.mode = Constants.MODE_FULL;
            int SubscriptionMode = response[0];
            tick.subscription_mode = Convert.ToUInt16(SubscriptionMode);
            tick.exchange_type = response[1];
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tick.token = parts[0];
            tick.sequence_number = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            var exchangeTimeStampInMilliSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tick.ExchangeTimestam = exchangeTimeStampInMilliSeconds;
            var ltp = BitConverter.ToInt64(response.Skip(43).Take(8).ToArray(), 0);
            tick.last_traded_price = ltp;
            tick.last_traded_quantity = BitConverter.ToInt64(response.Skip(51).Take(8).ToArray(), 0);
            var averageTradedPrice = BitConverter.ToInt64(response.Skip(59).Take(8).ToArray(), 0);
            tick.avg_traded_price = averageTradedPrice;
            tick.vol_traded = BitConverter.ToInt64(response.Skip(67).Take(8).ToArray(), 0);
            tick.total_buy_quantity = BitConverter.ToDouble(response.Skip(75).Take(8).ToArray(), 0);
            tick.total_sell_quantity = BitConverter.ToDouble(response.Skip(83).Take(8).ToArray(), 0);
            var openPriceOfTheDay = BitConverter.ToInt64(response.Skip(91).Take(8).ToArray(), 0);
            tick.open_price_day = openPriceOfTheDay;
            var highPriceOfTheDay = BitConverter.ToInt64(response.Skip(99).Take(8).ToArray(), 0);
            tick.high_price_day = highPriceOfTheDay;
            var lowPriceOfTheDay = BitConverter.ToInt64(response.Skip(107).Take(8).ToArray(), 0);
            tick.low_price_day = lowPriceOfTheDay;
            var closePrice = BitConverter.ToInt64(response.Skip(115).Take(8).ToArray(), 0);
            tick.close_price = closePrice;
            var epoch1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastTradedTimestampInSeconds = BitConverter.ToInt64(response.Skip(123).Take(8).ToArray(), 0);
            tick.last_traded_timestamp = lastTradedTimestampInSeconds;
            tick.open_interest = BitConverter.ToInt64(response.Skip(131).Take(8).ToArray(), 0);
            byte[] best5Bytes = response.Skip(147).Take(200).ToArray();
            tick.bestfivedata = new BestFiveItem[10];
            for (int i = 0; i < 10; i++)
            {
                var bestData = best5Bytes.Skip(i * 20).Take(20).ToArray();
                tick.bestfivedata[i].buy_sell_flag = BitConverter.ToInt16(bestData.Skip(0).Take(2).ToArray(), 0);

                tick.bestfivedata[i].quantity = BitConverter.ToInt64(bestData.Skip(2).Take(8).ToArray(), 0);

                var price = BitConverter.ToInt64(bestData.Skip(10).Take(8).ToArray(), 0);
                tick.bestfivedata[i].price = price;

                tick.bestfivedata[i].orders = BitConverter.ToInt16(bestData.Skip(18).Take(2).ToArray(), 0);
            }

            var upperCircuitLimit = BitConverter.ToInt64(response.Skip(347).Take(8).ToArray(), 0);
            tick.upper_circuit = upperCircuitLimit;
            var lowerCircuitLimit = BitConverter.ToInt64(response.Skip(355).Take(8).ToArray(), 0);
            tick.lower_circuit = lowerCircuitLimit;
            var fiftyTwoWeekHighPrice = BitConverter.ToInt64(response.Skip(363).Take(8).ToArray(), 0);
            tick.fiftytwo_week_high = fiftyTwoWeekHighPrice;
            var fiftyTwoWeekLowPrice = BitConverter.ToInt64(response.Skip(371).Take(8).ToArray(), 0);
            tick.fiftytwo_week_low = fiftyTwoWeekLowPrice;
            return tick;
        }

        /// <summary>
        /// Read a pong from websokect
        /// </summary>
        private TickPong ReadPong(byte[] response)
        {
            TickPong tickp = new TickPong();
            var result_msg = Encoding.UTF8.GetString(response.Skip(0).Take(4).ToArray());
            tickp.result = result_msg;
            return tickp;
        }

        /// <summary>
        /// Reads buffer data from raw binary data
        /// </summary>
        private void OnData(byte[] Data, bool EndOfMessage, string MessageType)
        {
            if (MessageType == Convert.ToString(WebSocketMessageType.Binary))
            {
                var sub_mod = Data.Skip(0).Take(1).ToArray();
                if (sub_mod[0] == Constants.LTP)
                {
                    TickLtp tickltp = new TickLtp();
                    tickltp = ReadLTP(Data);
                    OnTickLtp(tickltp);
                }
                else if (sub_mod[0] == Constants.QUOTE)
                {
                    TickQuote tickquote = new TickQuote();
                    tickquote = ReadQuote(Data);
                    OnTickQuote(tickquote);
                }
                else if (sub_mod[0] == Constants.SNAPQUOTE)
                {
                    Tick tick = new Tick();
                    tick = ReadFull(Data);
                    OnTick(tick);
                }
            }
            else if (MessageType == Convert.ToString(WebSocketMessageType.Text))
            {
                TickPong tickpong = new TickPong();
                tickpong = ReadPong(Data);
                OnTickPong(tickpong);
            }
            else
            {
                CloseForSocket();
            }
        }

        private void onConnect()
        {
            OnConnect?.Invoke();
            // Start the heartbeat method once connected
            Heartbeat();
        }
        /// <summary>
        /// Tells whether ticker is connected to server not.
        /// </summary>
        public bool IsConnectedforSocket
        {
            get { return _cws.IsConnectedForSocket(); }
        }

        /// <summary>
        /// Start a WebSocket connection
        /// </summary>
        public void Connect()
        {
            if (!IsConnectedforSocket)
            {
                _cws.Connect(socketUrl, new Dictionary<string, string>() { [Constants.AUTHORIZATION] = accessToken, [Constants.APIKEY] = apiKey, [Constants.CLIENTCODE] = clientCode, [Constants.FEEDTOKEN] = feedToken });

            }
        }

        /// <summary>
        /// Send Request message to WebSocket
        /// </summary>
        public void SendAsync(string msg)
        {
            if (IsConnectedforSocket)
            {
                _cws.SendAsync(msg);
            }
        }

        /// <summary>
        /// Receive response result from WebSocket
        /// </summary>
        public void ReceiveAsync()
        {
            if (IsConnectedforSocket)
            {
                try
                {
                    _cws.ReceiveAsync();
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    // Invoke the OnError event and pass the error message
                    OnError?.Invoke(errorMessage);
                }
            }
        }

        /// <summary>
        /// Close a WebSocket connection
        /// </summary>
        public void CloseForSocket()
        {
            _cws.CloseForSocket();

        }

        /// <summary>
        /// Send Request Message to Websocket
        /// </summary>
        public void SetMode(string requestMessage)
        {
            if (requestMessage == null) return;

            if (IsConnectedforSocket)
            {
                _cws.SendAsync(requestMessage).Wait();
            }
            if (IsConnectedforSocket)
            {
                _cws.ReceiveAsync().Wait();
            }
        }
        public void InitiatePingTimer()
        {
            // Start the ping timer
            pingTimer = new Timer(HandlePingTimeout, null, TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);
        }

        public void ResetPingTimer()
        {
            // Cancel the previous timer if it exists
            pingTimer?.Dispose();

            // Create a new timer to check for ping within 20 seconds
            pingTimer = new Timer(HandlePingTimeout, null, TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);
        }

        public void HandlePingTimeout(object state)
        {
            // Ping timeout occurred, handle it accordingly
            Console.WriteLine("Ping timeout. Reconnecting...");

            //Thread.Sleep(10000);
            // Perform necessary actions to handle the timeout and attempt to reconnect or handle the situation accordingly
            // For example, you can call the OnError method to initiate the reconnection process
            OnError("Ping timeout");
            ResetPingTimer();
        }
        public void Heartbeat()
        {
            int interval = 10; // Interval in seconds

            // Create a timer with the specified interval
            timer = new System.Timers.Timer(interval * 1000);

            timer.Elapsed += (sender, e) =>
            {
                if (IsConnectedforSocket)
                {
                    _cws.SendAsync(Constants.PING);
                }

                if (IsConnectedforSocket)
                {
                    _cws.ReceiveAsync();
                }
            };

            // Start the timer
            timer.Start();
        }


    }
    #endregion
}
