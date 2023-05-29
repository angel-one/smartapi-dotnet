using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace AngelBroking
{  
    public class SmartApi
    {
        protected string USER = "USER", SourceID = "WEB", PrivateKey = "";
        static string ClientPublicIP = "", ClientLocalIP = "", MACAddress = "";

        //protected string APIURL = "https://openapisuat.angelbroking.com";
        protected string APIURL = "https://apiconnect.angelbroking.com"; //prod endpoint

        AngelToken Token { get; set; }

        /*Constructors*/
        public SmartApi(string _PrivateKey)
        {
            PrivateKey = _PrivateKey;

            ClientPublicIP = Helpers.GetPublicIPAddress();
            if (ClientPublicIP == "")
                ClientPublicIP = Helpers.GetPublicIPAddress();

            if (ClientPublicIP == "")
                ClientPublicIP = "106.193.147.98";

            ClientLocalIP = Helpers.GetLocalIPAddress();

            if (ClientLocalIP == "")
                ClientLocalIP = "127.0.0.1";

            if (Helpers.GetMacAddress() != null)
                MACAddress = Helpers.GetMacAddress().ToString();
            else
                MACAddress = "fe80::216e:6507:4b90:3719";
        }
        public SmartApi(string _PrivateKey, string _jwtToken = "", string _refreshToken = "")
        {
            PrivateKey = _PrivateKey;

            this.Token = new AngelToken();
            this.Token.jwtToken = _jwtToken;
            this.Token.refreshToken = _refreshToken;
            this.Token.feedToken = "";

            ClientPublicIP = Helpers.GetPublicIPAddress();
            if (ClientPublicIP == "")
                ClientPublicIP = Helpers.GetPublicIPAddress();

            if (ClientPublicIP == "")
                ClientPublicIP = "106.193.147.98";

            ClientLocalIP = Helpers.GetLocalIPAddress();

            if (ClientLocalIP == "")
                ClientLocalIP = "127.0.0.1";

            if (Helpers.GetMacAddress() != null)
                MACAddress = Helpers.GetMacAddress().ToString();
            else
                MACAddress = "fe80::216e:6507:4b90:3719";
        }

        /* Makes a POST request */
        private string POSTWebRequest(AngelToken agr, string URL, string Data)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.jwtToken);
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

        /* Makes a GET request */
        private string GETWebRequest(AngelToken agr, string URL)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.jwtToken);
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

        /* Validate Token data internally */
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

        /*User Calls*/
        public OutputBaseClass GenerateSession(string clientcode, string password)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelTokenResponse agr = new AngelTokenResponse();

                string URL = APIURL + "/rest/auth/angelbroking/user/v1/loginByPassword";

                string PostData = "{\"clientcode\":\"" + clientcode + "\",\"password\":\"" + password + "\"}";

                string json = POSTWebRequest(null, URL, PostData);
                if (!json.Contains("PostError:"))
                {
                    agr = JsonConvert.DeserializeObject<AngelTokenResponse>(json);
                    res.TokenResponse = agr.data;
                    res.status = agr.status;
                    res.http_error = agr.message;
                    res.http_code = agr.errorcode;
                    this.Token = agr.data;
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = json.Replace("PostError:", "");
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass GenerateToken()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            AngelTokenResponse restoken = new AngelTokenResponse();
            try
            {
                AngelToken Token = this.Token;
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
                            res.TokenResponse = restoken.data;
                            res.status = restoken.status;
                            res.http_error = restoken.message;
                            res.http_code = restoken.errorcode;
                            this.Token = restoken.data;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass GetProfile()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/getProfile";

                        string json = GETWebRequest(Token, URL);
                        if (!json.Contains("GetError:"))
                        {
                            GetProfileResponse gres = JsonConvert.DeserializeObject<GetProfileResponse>(json);
                            res.GetProfileResponse = gres;
                            res.status = gres.status;
                            res.http_error = gres.message;
                            res.http_code = gres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }

        /*Logout Calls */
        public OutputBaseClass LogOut(string clientcode)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/logout";

                        string PostData = "{\"clientcode\":\"" + clientcode + "\"}";

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            LogOutResponse logres = JsonConvert.DeserializeObject<LogOutResponse>(Json);
                            res.LogOutResponse = logres;
                            res.status = logres.status;
                            res.http_error = logres.message;
                            res.http_code = logres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }

        /*Funds and Margins Calls*/
        public OutputBaseClass getRMS()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/user/v1/getRMS";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            GetRMSLimitResponse rms = JsonConvert.DeserializeObject<GetRMSLimitResponse>(Json);
                            res.GetRMSLimitResponse = rms;
                            res.status = rms.status;
                            res.http_error = rms.message;
                            res.http_code = rms.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }

        /*Orders Calls*/
        public OutputBaseClass placeOrder(OrderInfo order )
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/placeOrder";

                        if (order.triggerprice == null || order.triggerprice == "")
                            order.triggerprice = "0";
                        if (order.squareoff == null || order.squareoff == "")
                            order.squareoff = "0";
                        if (order.stoploss == null || order.stoploss == "")
                            order.stoploss = "0";
                        if (order.trailingStopLoss == null || order.trailingStopLoss == "")
                            order.trailingStopLoss = "0";
                        if (order.disclosedquantity == null || order.disclosedquantity == "")
                            order.disclosedquantity = "0";
                        if (order.ordertag == null)
                            order.ordertag = "";

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.PlaceOrderResponse = pres;
                            res.status = pres.status;
                            res.http_error = pres.message;
                            res.http_code = pres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }
        public OutputBaseClass modifyOrder(OrderInfo order )
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/modifyOrder";

                        if (order.triggerprice == null || order.triggerprice == "")
                            order.triggerprice = "0";
                        if (order.squareoff == null || order.squareoff == "")
                            order.squareoff = "0";
                        if (order.stoploss == null || order.stoploss == "")
                            order.stoploss = "0";
                        if (order.trailingStopLoss == null || order.trailingStopLoss == "")
                            order.trailingStopLoss = "0";
                        if (order.disclosedquantity == null || order.disclosedquantity == "")
                            order.disclosedquantity = "0";
                        if (order.ordertag == null)
                            order.ordertag = "";

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.ModifyOrderResponse = pres;
                            res.status = pres.status;
                            res.http_error = pres.message;
                            res.http_code = pres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass cancelOrder(OrderInfo order )
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/cancelOrder";

                        string PostData = "{\"variety\":\"" + order.variety + "\",\"orderid\":\"" + order.orderid + "\"}";

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.CancelOrderResponse = pres;
                            res.status = pres.status;
                            res.http_error = pres.message;
                            res.http_code = pres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getOrderBook()
        {

            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getOrderBook";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            GetOrderBookResponse bres = JsonConvert.DeserializeObject<GetOrderBookResponse>(Json);
                            res.GetOrderBookResponse = bres;
                            res.status = bres.status;
                            res.http_error = bres.message;
                            res.http_code = bres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }
        public OutputBaseClass getTradeBook()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getTradeBook";


                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            GetTradeBookResponse tres = JsonConvert.DeserializeObject<GetTradeBookResponse>(Json);
                            res.GetTradeBookResponse = tres;
                            res.status = tres.status;
                            res.http_error = tres.message;
                            res.http_code = tres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass GetLTPData(LTPDataRequest req )
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getLtpData";

                        string PostData = JsonConvert.SerializeObject(req);

                        string Json = POSTWebRequest(Token, URL, PostData);
                        if (!Json.Contains("PostError:"))
                        {
                            GetLTPDataResponse ltp = JsonConvert.DeserializeObject<GetLTPDataResponse>(Json);
                            res.GetLTPDataResponse = ltp;
                            res.status = ltp.status;
                            res.http_error = ltp.message;
                            res.http_code = ltp.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getHolding()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/portfolio/v1/getHolding";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            GetHoldingResponse hres = JsonConvert.DeserializeObject<GetHoldingResponse>(Json);
                            res.GetHoldingResponse = hres;
                            res.status = hres.status;
                            res.http_error = hres.message;
                            res.http_code = hres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass getPosition()
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/getPosition";

                        string Json = GETWebRequest(Token, URL);
                        if (!Json.Contains("GetError:"))
                        {
                            GetPositionResponse pres = JsonConvert.DeserializeObject<GetPositionResponse>(Json);
                            res.GetPositionResponse = pres;
                            res.status = pres.status;
                            res.http_error = pres.message;
                            res.http_code = pres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = Json.Replace("GetError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass convertPosition(ConvertPositionRequest Request )
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/order/v1/convertPosition";

                        string PostData = JsonConvert.SerializeObject(Request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            PositionConversionResponse pc = JsonConvert.DeserializeObject<PositionConversionResponse>(json);
                            res.PositionConversionResponse = pc;
                            res.status = pc.status;
                            res.http_error = pc.message;
                            res.http_code = pc.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }

        /* Create GTT Rule*/
        public OutputBaseClass CreateRule(CreateRuleRequest ruleRequest)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/createRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            RuleResponse rres = JsonConvert.DeserializeObject<RuleResponse>(json);
                            res.CreateRuleResponse = rres;
                            res.status = rres.status;
                            res.http_error = rres.message;
                            res.http_code = rres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass modifyRule(CreateRuleRequest ruleRequest)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/modifyRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            RuleResponse rres = JsonConvert.DeserializeObject<RuleResponse>(json);
                            res.ModifyRuleResponse = rres;
                            res.status = rres.status;
                            res.http_error = rres.message;
                            res.http_code = rres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass cancelRule(CancelRuleRequest ruleRequest)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/cancelRule";

                        string PostData = JsonConvert.SerializeObject(ruleRequest);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            RuleResponse rres = JsonConvert.DeserializeObject<RuleResponse>(json);
                            res.CancelRuleResponse = rres;
                            res.status = rres.status;
                            res.http_error = rres.message;
                            res.http_code = rres.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass ruleDetails(string RuleID)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/ruleDetails";

                        string PostData = "{\"id\":\"" + RuleID + "\"}";

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            RuleDetailsResponse rr = JsonConvert.DeserializeObject<RuleDetailsResponse>(json);
                            res.RuleDetailsResponse = rr;
                            res.status = rr.status;
                            res.http_error = rr.message;
                            res.http_code = rr.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
        public OutputBaseClass ruleList(RuleListRequest request)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/gtt/v1/ruleList";

                        string PostData = JsonConvert.SerializeObject(request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            RuleListResponse rr = JsonConvert.DeserializeObject<RuleListResponse>(json);
                            res.RuleListResponse = rr;
                            res.status = rr.status;
                            res.http_error = rr.message;
                            res.http_code = rr.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }

        /*Historical Data*/
        public OutputBaseClass GetCandleData(CandleRequest Request)
        {
            OutputBaseClass res = new OutputBaseClass();
            res.status = true;
            res.http_code = "200";
            try
            {
                AngelToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/rest/secure/angelbroking/historical/v1/getCandleData";

                        string PostData = JsonConvert.SerializeObject(Request);

                        string json = POSTWebRequest(Token, URL, PostData);
                        if (!json.Contains("PostError:"))
                        {
                            CandleDataResponse cd = JsonConvert.DeserializeObject<CandleDataResponse>(json);
                            res.GetCandleDataResponse = cd;
                            res.status = cd.status;
                            res.http_error = cd.message;
                            res.http_code = cd.errorcode;
                        }
                        else
                        {
                            res.status = false;
                            res.http_code = "404";
                            res.http_error = json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = false;
                        res.http_code = "404";
                        res.http_error = "The token is invalid";
                    }
                }
                else
                {
                    res.status = false;
                    res.http_code = "404";
                    res.http_error = "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;
        }
    }
}
