using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelBroking
{
    class AngelBrokingModel
    {
    }
    #region Websocketv1
    /* Output Classes*/
    public class AngelToken
    {
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }
        public string feedToken { get; set; }
    }
    public class AngelTokenResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public AngelToken data { get; set; }
    }
    public class ProfileData
    {
        public string clientcode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }
        public List<string> exchanges { get; set; }
        public List<string> products { get; set; }
        public string lastlogintime { get; set; }
        public string brokerid { get; set; }
    }
    public class GetProfileResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public ProfileData data { get; set; }
    }

    public class LogOutResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public string data { get; set; }
    }

    public class RMSLimitData
    {
        public string net { get; set; }
        public string availablecash { get; set; }
        public string availableintradaypayin { get; set; }
        public string availablelimitmargin { get; set; }
        public string collateral { get; set; }
        public string m2munrealized { get; set; }
        public string m2mrealized { get; set; }
        public string utiliseddebits { get; set; }
        public string utilisedspan { get; set; }
        public string utilisedoptionpremium { get; set; }
        public string utilisedholdingsales { get; set; }
        public string utilisedexposure { get; set; }
        public string utilisedturnover { get; set; }
        public string utilisedpayout { get; set; }
    }

    public class GetRMSLimitResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public RMSLimitData data { get; set; }
    }

    public class OrderData
    {
        public string script { get; set; }
        public long orderid { get; set; }
    }

    public class OrderResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public OrderData data { get; set; }
    }

    public class OrderBookData
    {
        public string variety { get; set; }
        public string ordertype { get; set; }
        public string producttype { get; set; }
        public string duration { get; set; }
        public string price { get; set; }
        public string triggerprice { get; set; }
        public string quantity { get; set; }
        public string disclosedquantity { get; set; }
        public string squareoff { get; set; }
        public string stoploss { get; set; }
        public string trailingstoploss { get; set; }
        public string tradingsymbol { get; set; }
        public string transactiontype { get; set; }
        public string exchange { get; set; }
        public string symboltoken { get; set; }
        public string instrumenttype { get; set; }
        public string strikeprice { get; set; }
        public string optiontype { get; set; }
        public string expirydate { get; set; }
        public string lotsize { get; set; }
        public string cancelsize { get; set; }
        public string averageprice { get; set; }
        public string filledshares { get; set; }
        public string unfilledshares { get; set; }
        public string orderid { get; set; }
        public string text { get; set; }
        public string status { get; set; }
        public string orderstatus { get; set; }
        public string updatetime { get; set; }
        public string exchtime { get; set; }
        public string exchorderupdatetime { get; set; }
        public string fillid { get; set; }
        public string filltime { get; set; }
        public string parentorderid { get; set; }
    }
    public class GetOrderBookResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<OrderBookData> data { get; set; }
    }
    public class TradeBookData
    {
        public string exchange { get; set; }
        public string producttype { get; set; }
        public string tradingsymbol { get; set; }
        public string instrumenttype { get; set; }
        public string symbolgroup { get; set; }
        public string strikeprice { get; set; }
        public string optiontype { get; set; }
        public string expirydate { get; set; }
        public string marketlot { get; set; }
        public string precision { get; set; }
        public string multiplier { get; set; }
        public string tradevalue { get; set; }
        public string transactiontype { get; set; }
        public string fillprice { get; set; }
        public string fillsize { get; set; }
        public string orderid { get; set; }
        public string fillid { get; set; }
        public string filltime { get; set; }
    }

    public class GetTradeBookResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<TradeBookData> data { get; set; }
    }

    public class LastTradedPrices
    {
        public string exchange { get; set; }
        public string tradingsymbol { get; set; }
        public string symboltoken { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string ltp { get; set; }
    }

    public class GetLTPDataResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public LastTradedPrices data { get; set; }
    }
    public class HoldingData
    {
        public string tradingSymbol { get; set; }
        public string exchange { get; set; }
        public string isin { get; set; }
        public long t1quantity { get; set; }
        public long realisedquantity { get; set; }
        public long quantity { get; set; }
        public long authorisedquantity { get; set; }
        public decimal profitandloss { get; set; }
        public string product { get; set; }
        public string collateralquantity { get; set; }
        public string collateraltype { get; set; }
        public decimal haircut { get; set; }
        public decimal averageprice { get; set; }
        public decimal ltp { get; set; }
        public decimal close { get; set; }
        public string symboltoken { get; set; }
    }

    public class GetHoldingResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<HoldingData> data { get; set; }
    }
    public class PositionData
    {
        public string exchange { get; set; }
        public string symboltoken { get; set; }
        public string producttype { get; set; }
        public string tradingsymbol { get; set; }
        public string symbolname { get; set; }
        public string instrumenttype { get; set; }
        public string priceden { get; set; }
        public string pricenum { get; set; }
        public string genden { get; set; }
        public string gennum { get; set; }
        public string precision { get; set; }
        public string multiplier { get; set; }
        public string boardlotsize { get; set; }
        public string buyqty { get; set; }
        public string sellqty { get; set; }
        public string buyamount { get; set; }
        public string sellamount { get; set; }
        public string symbolgroup { get; set; }
        public string strikeprice { get; set; }
        public string optiontype { get; set; }
        public string expirydate { get; set; }
        public string lotsize { get; set; }
        public string cfbuyqty { get; set; }
        public string cfsellqty { get; set; }
        public string cfbuyamount { get; set; }
        public string cfsellamount { get; set; }
        public string buyavgprice { get; set; }
        public string sellavgprice { get; set; }
        public string avgnetprice { get; set; }
        public string netvalue { get; set; }
        public string netqty { get; set; }
        public string totalbuyvalue { get; set; }
        public string totalsellvalue { get; set; }
        public string cfbuyavgprice { get; set; }
        public string cfsellavgprice { get; set; }
        public string totalbuyavgprice { get; set; }
        public string totalsellavgprice { get; set; }
        public string netprice { get; set; }
    }

    public class GetPositionResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<PositionData> data { get; set; }
    }
    public class PositionConversionResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public object data { get; set; }
    }
    public class RuleData
    {
        public int id { get; set; }
    }

    public class RuleResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public RuleData data { get; set; }
    }
    public class RuleDetail
    {
        public string status { get; set; }
        public DateTime createddate { get; set; }
        public DateTime updateddate { get; set; }
        public DateTime expirydate { get; set; }
        public string clientid { get; set; }
        public string tradingsymbol { get; set; }
        public string symboltoken { get; set; }
        public string exchange { get; set; }
        public string producttype { get; set; }
        public string transactiontype { get; set; }
        public double price { get; set; }
        public int qty { get; set; }
        public double triggerprice { get; set; }
        public int disclosedqty { get; set; }
    }

    public class RuleListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<RuleDetail> data { get; set; }
    }
    public class RuleDetailsResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public RuleDetail data { get; set; }
    }

    public class CandleDataResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public List<List<object>> data { get; set; }
    }
    public class OutputBaseClass
    {
        public bool status { get; set; }
        public string http_code { get; set; }
        public string http_error { get; set; }
        public AngelToken TokenResponse { get; set; }
        //public AngelTokenResponse TokenResponse { get; set; }
        public GetProfileResponse GetProfileResponse { get; set; }
        public LogOutResponse LogOutResponse { get; set; }
        public GetRMSLimitResponse GetRMSLimitResponse { get; set; }
        public OrderResponse PlaceOrderResponse { get; set; }
        public OrderResponse ModifyOrderResponse { get; set; }
        public OrderResponse CancelOrderResponse { get; set; }
        public GetOrderBookResponse GetOrderBookResponse { get; set; }
        public GetTradeBookResponse GetTradeBookResponse { get; set; }
        public GetLTPDataResponse GetLTPDataResponse { get; set; }
        public GetHoldingResponse GetHoldingResponse { get; set; }
        public GetPositionResponse GetPositionResponse { get; set; }
        public PositionConversionResponse PositionConversionResponse { get; set; }
        public RuleResponse CreateRuleResponse { get; set; }
        public RuleResponse ModifyRuleResponse { get; set; }
        public RuleResponse CancelRuleResponse { get; set; }
        public RuleDetailsResponse RuleDetailsResponse { get; set; }
        public RuleListResponse RuleListResponse { get; set; }
        public CandleDataResponse GetCandleDataResponse { get; set; }
    }

    /* Input Classes*/
    public class OrderInfo
    {
        public string orderid { get; set; }
        public string variety { get; set; }
        public string tradingsymbol { get; set; }
        public string symboltoken { get; set; }
        public string transactiontype { get; set; }
        public string exchange { get; set; }
        public string ordertype { get; set; }
        public string producttype { get; set; }
        public string duration { get; set; }
        public string price { get; set; }
        public string squareoff { get; set; }
        public string stoploss { get; set; }
        public string quantity { get; set; }
        public string triggerprice { get; set; }
        public string trailingStopLoss { get; set; }
        public string disclosedquantity { get; set; }
        public string ordertag { get; set; }
    }

    public class ConvertPositionRequest
    {
        public string exchange { get; set; }
        public string oldproducttype { get; set; }
        public string newproducttype { get; set; }
        public string tradingsymbol { get; set; }
        public string transactiontype { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
    }

    public class CreateRuleRequest
    {
        public string id { get; set; }
        public string tradingsymbol { get; set; }
        public string symboltoken { get; set; }
        public string exchange { get; set; }
        public string transactiontype { get; set; }
        public string producttype { get; set; }
        public string price { get; set; }
        public string qty { get; set; }
        public string triggerprice { get; set; }
        public string disclosedqty { get; set; }
        public string timeperiod { get; set; }
    }
    public class CancelRuleRequest
    {
        public string id { get; set; }
        public string symboltoken { get; set; }
        public string exchange { get; set; }
    }
    public class RuleListRequest
    {
        public List<string> status { get; set; }
        public int page { get; set; }
        public int count { get; set; }
    }

    public class CandleRequest
    {
        public string symboltoken { get; set; }
        public string exchange { get; set; }
        public string interval { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }

    public class LTPDataRequest
    {
        public string symboltoken { get; set; }
        public string exchange { get; set; }
        public string tradingsymbol { get; set; }
    }
    #endregion

    #region WebSocketv2
    public class TokenParams
    {
        public int mode { get; set; }
        public List<TokenList> tokenList { get; set; }
    }
    public class TickerRequest
    {
        public string correlationID { get; set; }
        public int action { get; set; }

        [JsonPropertyName("params")]
        public TokenParams param { get; set; }
    }
    public class TokenList
    {
        public int exchangeType { get; set; }
        public List<string> tokens { get; set; }

       
    }
    #endregion
}
