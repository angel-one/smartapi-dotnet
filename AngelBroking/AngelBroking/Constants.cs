using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBroking
{
    public class Constants
    {
        #region Websocketv1
        // Products
        public const string PRODUCT_TYPE_DELIVERY = "DELIVERY";
        public const string PRODUCT_TYPE_MARGIN = "MARGIN";
        public const string PRODUCT_TYPE_INTRADAY = "INTRADAY";
        public const string PRODUCT_TYPE_BO = "BO";
        public const string PRODUCT_TYPE_CARRYFORWARD = "CARRYFORWARD";
        // Order types
        public const string ORDER_TYPE_MARKET = "MARKET";
        public const string ORDER_TYPE_LIMIT = "LIMIT";
        public const string ORDER_TYPE_STOPLOSS_LIMIT = "STOPLOSS_LIMIT";
        public const string ORDER_TYPE_STOPLOSS_MARKET = "STOPLOSS_MARKET";

        // Varities
        public const string VARIETY_NORMAL = "NORMAL";
        public const string VARIETY_STOPLOSS = "STOPLOSS";
        public const string VARIETY_ROBO = "ROBO";
        public const string VARIETY_AMO = "AMO";

        // Transaction type
        public const string TRANSACTION_TYPE_BUY = "BUY";
        public const string TRANSACTION_TYPE_SELL = "SELL";

        // Validity
        public const string VALIDITY_DAY = "DAY";
        public const string VALIDITY_IOC = "IOC";

        // Exchanges
        public const string EXCHANGE_NSE = "NSE";
        public const string EXCHANGE_BSE = "BSE";
        public const string EXCHANGE_NFO = "NFO";
        public const string EXCHANGE_CDS = "CDS";
        public const string EXCHANGE_MCX = "MCX";
        public const string EXCHANGE_NCDEX = "NCDEX";


        public const string INTERVAL_MINUTE = "ONE_MINUTE";
        public const string INTERVAL_THREE_MINUTE = "THREE_MINUTE";
        public const string INTERVAL_FIVE_MINUTE = "FIVE_MINUTE";
        public const string INTERVAL_TEN_MINUTE = "TEN_MINUTE";
        public const string INTERVAL_FIFTEEN_MINUTE = "FIFTEEN_MINUTE";
        public const string INTERVAL_THIRTY_MINUTE = "THIRTY_MINUTE";
        public const string INTERVAL_ONE_HOUR = "ONE_HOUR";
        public const string INTERVAL_ONE_DAY = "ONE_DAY";
        #endregion

        #region WebSocketv2
        // Ticker modes
        public const string MODE_FULL = "SnapQuote";
        public const string MODE_QUOTE = "Quote";
        public const string MODE_LTP = "LTP";

        public const string PING = "ping";

        public const int SNAPQUOTE = 3;
        public const int QUOTE = 2;
        public const int LTP = 1;

        // WS URl
        public const string ROOT = "ws://smartapisocket.angelone.in/smart-stream";

        //Header parameters
        public const string  AUTHORIZATION = "Authorization";
        public const string  APIKEY = "x-api-key";
        public const string  CLIENTCODE = "x-client-code";
        public const string  FEEDTOKEN = "x-feed-token";
        #endregion
    }
}
