using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace smartapi_dotnet
{
    #region  AngelBrokingModel
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

    public class OutputBaseClass
    {
        public bool status { get; set; }
        public Int32 http_code { get; set; }
        public string http_error { get; set; }
        public object response_data { get; set; }

    }


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
        public string symboltoken { get; set; }
        public string producttype { get; set; }
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
    #endregion

    public class AngelBroking
    {
        protected string APIURL = "", USER = "", SourceID = "", ClientLocalIP = "", ClientPublicIP = "", MACAddress = "", PrivateKey = "";

        public AngelBroking(string URL, string U, string S, string CL, string CP, string MA, string PK)
        {
            APIURL = URL;
            USER = U;
            SourceID = S;
            ClientLocalIP = CL;
            ClientPublicIP = CP;
            MACAddress = MA;
            PrivateKey = PK;
        }

        public string POSTWebRequest(AngelToken agr, string URL, string Data)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.jwtToken);
                //httpWebRequest = addHeaders(httpWebRequest);
                httpWebRequest.Headers.Add("X-Content-Type-Options", "nosniff");
                httpWebRequest.Headers.Add("X-UserType", USER);
                httpWebRequest.Headers.Add("X-SourceID", SourceID);
                httpWebRequest.Headers.Add("X-ClientLocalIP", ClientLocalIP);
                httpWebRequest.Headers.Add("X-ClientPublicIP", ClientPublicIP);
                httpWebRequest.Headers.Add("X-MACAddress", MACAddress);
                httpWebRequest.Headers.Add("X-PrivateKey", PrivateKey);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes(Data);
                httpWebRequest.ContentLength = byteArray.Length;
                string Json = "";

                Stream dataStream = httpWebRequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "PostError:" + ex.Message;
            }
        }
        public string GETWebRequest(AngelToken agr, string URL)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.jwtToken);
                //httpWebRequest = addHeaders(httpWebRequest);
                httpWebRequest.Headers.Add("X-Content-Type-Options", "nosniff");
                httpWebRequest.Headers.Add("X-UserType", USER);
                httpWebRequest.Headers.Add("X-SourceID", SourceID);
                httpWebRequest.Headers.Add("X-ClientLocalIP", ClientLocalIP);
                httpWebRequest.Headers.Add("X-ClientPublicIP", ClientPublicIP);
                httpWebRequest.Headers.Add("X-MACAddress", MACAddress);
                httpWebRequest.Headers.Add("X-PrivateKey", PrivateKey);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                string Json = "";
                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "GetError:" + ex.Message;
            }
        }
        private bool ValidateToken(AngelToken token)
        {
            bool result = false;
            if (token != null)
            {
                if (token.jwtToken != "" && token.refreshToken != "")
                {
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }

        public OutputBaseClass GenerateSession(string clientcode, string password)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                AngelTokenResponse agr = new AngelTokenResponse();

                string URL = APIURL + "/rest/auth/angelbroking/user/v1/loginByPassword";

                string PostData = "{\"clientcode\":\"" + clientcode + "\",\"password\":\"" + password + "\"}";

                string json = POSTWebRequest(null, URL, PostData);
                if (!json.Contains("PostError:"))
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(json);
                    res.response_data = agr;
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = json.Replace("PostError:", "");
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass GenerateToken(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse restoken = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {                    
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/auth/angelbroking/jwt/v1/generateTokens";

                        string PostData = "{\"refreshToken\":\"" + Token.refreshToken + "\"}";

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            restoken = JsonConvert.DeserializeObject<AngelTokenResponse>(json);
                            res.response_data = restoken;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass GetProfile(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/getProfile";

                        string json = GETWebRequest(Token, URL);
                        if (!json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass LogOut(string clientcode, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {                    
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/logout";

                        string PostData = "{\"clientcode\":\"" + clientcode + "\"}";

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }

        public OutputBaseClass placeOrder(OrderInfo order, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {                   
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/placeOrder";

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;

        }
        public OutputBaseClass modifyOrder(OrderInfo order, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/modifyOrder";

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass cancelOrder(OrderInfo order, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/cancelOrder";

                        string PostData = "{\"variety\":\"" + order.variety + "\",\"orderid\":\"" + order.orderid + "\"}";

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }

        public OutputBaseClass getOrderBook(AngelToken Token)
        {

            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getOrderBook";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;

        }
        public OutputBaseClass getTradeBook(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getTradeBook";


                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getHolding(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(Token.ToString());
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/portfolio/v1/getHolding";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getPosition(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(Token.ToString());
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getPosition";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getRMS(AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/getRMS";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(Json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass convertPosition(ConvertPositionRequest Request, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/convertPosition";

                        string PostData = JsonConvert.SerializeObject(Request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        /* Create GTT Rule*/
        public OutputBaseClass CreateRule(CreateRuleRequest ruleRequest, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/createRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass modifyRule(CreateRuleRequest ruleRequest, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/modifyRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass cancelRule(CreateRuleRequest ruleRequest, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/cancelRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass ruleDetails(string RuleID, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(Token.ToString());
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/ruleDetails";

                        string PostData = "{\"id\":\"" + RuleID + "\"}";

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass ruleList(RuleListRequest request, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;
            AngelTokenResponse agr = new AngelTokenResponse();
            try
            {
                if (Token != null)
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(Token.ToString());
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/ruleList";

                        string PostData = JsonConvert.SerializeObject(request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }

        public OutputBaseClass GetCandleData(CandleRequest Request, AngelToken Token)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = 200;            
            try
            {
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/historical/v1/getCandleData";

                        string PostData = JsonConvert.SerializeObject(Request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            res.response_data = JsonConvert.DeserializeObject<object>(json);
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = 404;
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = 404;
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = 404;
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = 404;
                res.http_error = ex.Message;
            }
            return res;
        }
    }
}
