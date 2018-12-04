using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace PSMS_Sub_WebAPI.Controllers.Payment
{
    [WebApiTracker, Authenticate]
    public class PaymentController : ApiController
    {
        private PAYMENT_BL _PAYMENT_BL;
        protected PAYMENT_BL BL_PAYMENT
        {
            get { return _PAYMENT_BL ?? (_PAYMENT_BL = new PAYMENT_BL()); }
        }

        private SendEmailHelper _Email_Helper;
        protected SendEmailHelper Helper_Email
        {
            get
            {
                return _Email_Helper ?? (_Email_Helper = new SendEmailHelper());
            }
        }

        /// <summary>
        /// Get Product post form
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("Payment/GetProductPostForm")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetProductPostForm(string category)
        {
            return await Task.FromResult(GetProductPostForm(category));
        }

        public IHttpActionResult GetProductPostForm(string category)
        {
            try
            {
                DataSet ds = BL_PAYMENT.vmQueryBySeletProductPostForm(category);
                if (CacheWrapper.CurrentMultilingual.MultilingualID == "1")
                {
                    ds.Tables[0].Rows[0]["LANG"] = "C";
                }
                else if (CacheWrapper.CurrentMultilingual.MultilingualID == "3")
                {
                    ds.Tables[0].Rows[0]["LANG"] = "X";
                }
                return Ok(ds);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        [Route("Payment/CreateOrder")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> pCreateOrder(PAYMENT payment)
        {
            return await Task.FromResult(CreateOrder(payment));
        }
        public IHttpActionResult CreateOrder(PAYMENT payment)
        {
            try
            {
                payment.CUSTOMER_ID = SessionWrapper.CurrentUser.CustomerID;
                payment.MULTILINGUAL_ID = CacheWrapper.CurrentMultilingual.MultilingualID;
                return Ok(BL_PAYMENT.vmQueryByCreateOrder(payment));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain_name"></param>
        /// <returns></returns>
        [Route("Payment/CheckDomain")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pCheckDomainName(string domain_name)
        {
            return await Task.FromResult(CheckDomainName(domain_name));
        }

        public IHttpActionResult CheckDomainName(string domain_name)
        {
            try
            {
                return Ok(BL_PAYMENT.vmQueryByCheckDomainName(domain_name));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }


        [Route("Payment/CheckRenew")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pCheckIsCanRenew(string orederref)
        {
            return await Task.FromResult(CheckIsCanRenew(orederref));
        }

        public IHttpActionResult CheckIsCanRenew(string orederref)
        {
            try
            {
                return Ok(BL_PAYMENT.vmQueryByCheckIsCanRenew(orederref));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        // Add by chester 2018-10-22
        [Route("Payment/AutoPaymentDetail")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pAutoPaymentGetDetail(string schPayId)
        {
            return await Task.FromResult(AutoPaymentGetDetail(schPayId));
        }
        public IHttpActionResult AutoPaymentGetDetail(string schPayId)
        {
            try
            {
                return Ok(BL_PAYMENT.vmQueryDetailSchPay(schPayId).Tables[0]);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }


        [Route("Payment/AutoPaymentMethod")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pAutoPaymentGetMethod()
        {
            return await Task.FromResult(AutoPaymentGetMethod());
        }
        public IHttpActionResult AutoPaymentGetMethod()
        {
            try
            {
                return Ok(BL_PAYMENT.vmGetPaymentMethod());
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        // End


        /// <summary>
        /// Add by bill 2018-10-18
        /// </summary>
        /// <param name="orderRef"></param>
        /// <returns></returns>
        [Route("Payment/AutoPayment")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pAutoPaymentGetInfo(string orderRef)
        {
            return await Task.FromResult(AutoPaymentGetInfo(orderRef));
        }


        public IHttpActionResult AutoPaymentGetInfo(string orderRef)
        {
            try
            {
                DataSet ds = BL_PAYMENT.vmQueryOrderPayRef(orderRef);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GetOrderPaymentStatus(orderRef, ds.Tables[0].Rows[0]["PAYREF"].ToString());
                }
                // Add by chester 2018-10-19

                return Ok(BL_PAYMENT.vmQueryAutoPayMessage(orderRef).Tables[0]);
            }
            catch (Exception e)
            {
                return Ok(e);
            }


            // End 
        }

        public void GetOrderPaymentStatus(string orderRef, string payRef)
        {
            string QueryOrderStatusUrl = ConfigurationManager.AppSettings["AisaPayApiUrl"].ToString().Trim() + "?merchantId=" + ConfigurationManager.AppSettings["AisaMerchantId"].ToString().Trim() + "&loginId=" + ConfigurationManager.AppSettings["AisaApiLoginId"].ToString().Trim() + "&password=" + ConfigurationManager.AppSettings["AisaApiPassword"].ToString().Trim() + "&actionType=Query&orderRef=" + orderRef + "&payRef=" + payRef;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(QueryOrderStatusUrl);
            //request.Method = "GET";
            //request.ContentType = "text/html;charset=UTF-8";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream OrderStatusResponseStream = response.GetResponseStream();
            //StreamReader OrderStatusStreamReader = new StreamReader(OrderStatusResponseStream, Encoding.GetEncoding("utf-8"));
            //string XMLString = OrderStatusStreamReader.ReadToEnd();            
            //OrderStatusStreamReader.Close();
            //OrderStatusResponseStream.Close();
            string XMLString = PSMS_Utility.HttpMethod.HttpGetRequestMethod(QueryOrderStatusUrl);
            string[] startStr = { "<orderStatus>", "<ref>", "<payRef>", "<amt>", "<cur>", "<prc>", "<src>", "<ord>", "<holder>", "<authId>", "<alertCode>", "<remark>", "<eci>", "<payerAuth>", "<sourceIp>", "<ipCountry>", "<payMethod>", "<panFirst4>", "<panLast4>", "<cardIssuingCountry>", "<channelType>", "<txTime>", "<successcode>", "<mSchPayId>", "<dSchPayId>", "<MerchantId>", "<errMsg>" };
            string[] endStr = { "</orderStatus>", "</ref>", "</payRef>", "</amt>", "</cur>", "</prc>", "</src>", "</ord>", "</holder>", "</authId>", "</alertCode>", "</remark>", "</eci>", "</payerAuth>", "</sourceIp>", "</ipCountry>", "</payMethod>", "</panFirst4>", "</panLast4>", "</cardIssuingCountry>", "</channelType>", "</txTime>", "</successcode>", "</mSchPayId>", "</dSchPayId>", "</MerchantId>", "</errMsg>" };
            string[] XmlFiledValues = new string[startStr.Length];
            for (int i = 0; i < startStr.Length; i++)
            {
                Regex rg = new Regex("(?<=(" + startStr[i] + "))[.\\s\\S]*?(?=(" + endStr[i] + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                XmlFiledValues[i] = rg.Match(XMLString).Value;
            }
            //GetSchPaymentInfo(XmlFiledValues[23]);
            GetSchPaymentInfo(BL_PAYMENT.vmSaveQueryPaymentStatus(XmlFiledValues));
        }

        public void GetSchPaymentInfo(string mSchPayId)
        {
            string QuerySchPayUrl = ConfigurationManager.AppSettings["AisaPayQuerySchPayUrl"].ToString().Trim() + "?merchantId=" + ConfigurationManager.AppSettings["AisaMerchantId"].ToString().Trim() + "&loginId=" + ConfigurationManager.AppSettings["AisaApiLoginId"].ToString().Trim() + "&password=" + ConfigurationManager.AppSettings["AisaApiPassword"].ToString().Trim() + "&actionType=Query&mSchPayId=" + mSchPayId;
            //HttpWebRequest SchPayRequest = (HttpWebRequest)WebRequest.Create(QuerySchPayUrl);
            //SchPayRequest.Method = "GET";
            //SchPayRequest.ContentType = "text/html;charset=UTF-8";
            //HttpWebResponse SchPayResponse = (HttpWebResponse)SchPayRequest.GetResponse();
            //Stream SchPayResponseStream = SchPayResponse.GetResponseStream();
            //StreamReader SchPayStreamReader = new StreamReader(SchPayResponseStream, Encoding.GetEncoding("utf-8"));
            //string SchPayXMLString = SchPayStreamReader.ReadToEnd();
            //SchPayStreamReader.Close();
            //SchPayResponseStream.Close();
            string SchPayXMLString = PSMS_Utility.HttpMethod.HttpGetRequestMethod(QuerySchPayUrl);
            string[] SchstartStr = { "<mSchPayId>", "<schType>", "<startDate>", "<endDate>", "<merRef>", "<amount>", "<payType>", "<payMethod>", "<account>", "<holder>", "<expiryDate>", "<status>", "<suspendDate>", "<lastTerminateDate>", "<reActivateDate>", "<detailSchPay>" };
            string[] SchendStr = { "</mSchPayId>", "</schType>", "</startDate>", "</endDate>", "</merRef>", "</amount>", "</payType>", "</payMethod>", "</account>", "</holder>", "</expiryDate>", "</status>", "</suspendDate>", "</lastTerminateDate>", "</reActivateDate>", "<detailSchPay>" };
            string[] SchXmlFiledValues = new string[SchstartStr.Length];
            for (int i = 0; i < SchstartStr.Length; i++)
            {
                Regex Schrg = new Regex("(?<=(" + SchstartStr[i] + "))[.\\s\\S]*?(?=(" + SchendStr[i] + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                SchXmlFiledValues[i] = Schrg.Match(SchPayXMLString).Value;
            }
            BL_PAYMENT.vmSaveQuerySchPaymentInfo(SchXmlFiledValues);
            GetDetailSchPaymentInfo(SchPayXMLString, mSchPayId);
        }

        public void GetDetailSchPaymentInfo(string SchPayXMLString, string mSchPayId)
        {
            XmlDocument SchPayXml = new XmlDocument();
            SchPayXml.LoadXml(SchPayXMLString);
            XmlNodeList xe = SchPayXml.SelectNodes("records/masterSchPay/detailSchPay");
            string[] DetailSchstartStr = { "<dSchPayId>", "<schType>", "<orderDate>", "<tranDate>", "<currency>", "<amount>", "<status>", "<payRef>" };
            string[] DetailSchendStr = { "</dSchPayId>", "</schType>", "</orderDate>", "</tranDate>", "</currency>", "</amount>", "</status>", "</payRef>" };
            string[] DetailSchXmlFiledValues = new string[DetailSchstartStr.Length];
            for (int i = 0; i < xe.Count; i++)
            {
                for (int y = 0; y < DetailSchstartStr.Length; y++)
                {
                    Regex DetailSchrg = new Regex("(?<=(" + DetailSchstartStr[y] + "))[.\\s\\S]*?(?=(" + DetailSchendStr[y] + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                    DetailSchXmlFiledValues[y] = DetailSchrg.Match(xe[i].InnerXml).Value;
                }
                BL_PAYMENT.vmSaveQueryDetailSchPaymentInfo(DetailSchXmlFiledValues, mSchPayId);
            }
        }
        //End

        /// <summary>
        /// Add by bill 2018-10-23
        /// </summary>
        /// <param name="pMethod"></param>
        /// <param name="orderAcct"></param>
        /// <param name="holderName"></param>
        /// <param name="expireDate"></param>
        /// <returns></returns>
        [Route("Payment/UpdatePaymentCardInfo")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pUpdatePaymentCardInfo(string orderRef, string mSchPayId, string pMethod, string orderAcct, string holderName, string expireDate)
        {
            return await Task.FromResult(UpdatePaymentCardInfo(orderRef, mSchPayId, pMethod, orderAcct, holderName, expireDate));
        }

        public IHttpActionResult UpdatePaymentCardInfo(string orderRef, string mSchPayId, string pMethod, string orderAcct, string holderName, string expireDate)
        {

            try
            {
                string oldCard = BL_PAYMENT.vmQuerySchPaymentAccount(orderRef);
                string[] expireDateStr = Regex.Split(expireDate, "-", RegexOptions.IgnoreCase);
                string UpdateUrl = ConfigurationManager.AppSettings["AisaPayQuerySchPayUrl"].ToString().Trim() + "?merchantId=" + ConfigurationManager.AppSettings["AisaMerchantId"].ToString().Trim() + "&loginId=" + ConfigurationManager.AppSettings["AisaApiLoginId"].ToString().Trim() + "&password=" + ConfigurationManager.AppSettings["AisaApiPassword"].ToString().Trim() + "&actionType=UpdateSchPayCard&mSchPayId=" + mSchPayId + "&pMethod=" + pMethod + "&orderAcct=" + orderAcct + "&holderName=" + holderName + "&epMonth=" + expireDateStr[1] + "&epYear=" + expireDateStr[0];
                string returnString = PSMS_Utility.HttpMethod.HttpGetRequestMethod(UpdateUrl);
                string[] sArray = Regex.Split(returnString, "&", RegexOptions.IgnoreCase);
                for (int i = 0; i < sArray.Length; i++)
                {
                    sArray[i] = sArray[i].Split('=')[1];
                }
                 BL_PAYMENT.vmSaveChangeCardInfoFeedBack(sArray);
                if (returnString.Contains("Update successfully"))
                {
                    BL_PAYMENT.vmUpdateCardInfoFeedBack(orderRef, sArray[1]);
                    // Add by Chester 2018.10.30
                    // Send email after modifying the bank card successfully

                    Helper_Email.SendEmailAfterModifyingBankCardSuccess(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, oldCard, orderAcct, CacheWrapper.CurrentMultilingual.MultilingualID);
                    // End
                    return Ok(0);
                }
                else if (returnString.Contains("cardNo incorrect"))
                {
                    return Ok(100);
                }
                else if (returnString.Contains("cardNo don't match card Type"))
                {
                    return Ok(200);
                }
                else
                {
                    return Ok(300);
                }

            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }


        [Route("Payment/CancelPaymentOrder")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pCancelSchPaymentRecord(string orderRef)
        {
            return await Task.FromResult(CancalSchPaymentRecord(orderRef));
        }

        public IHttpActionResult CancalSchPaymentRecord(string orderRef)
        {
             string mSchPayId = BL_PAYMENT.vmQuerySchPaymentmSchPayId(orderRef);
            string CancelUrl = ConfigurationManager.AppSettings["AisaPayQuerySchPayUrl"].ToString().Trim() + "?merchantId=" + ConfigurationManager.AppSettings["AisaMerchantId"].ToString().Trim() + "&loginId=" + ConfigurationManager.AppSettings["AisaApiLoginId"].ToString().Trim() + "&password=" + ConfigurationManager.AppSettings["AisaApiPassword"].ToString().Trim() + "&actionType=DeleteSchPay&mSchPayId=" + mSchPayId;
            string returnString = PSMS_Utility.HttpMethod.HttpGetRequestMethod(CancelUrl);
            string[] sArray = Regex.Split(returnString, "&", RegexOptions.IgnoreCase);
            for (int i = 0; i < sArray.Length; i++)
            {
                sArray[i] = sArray[i].Split('=')[1];
            }
            BL_PAYMENT.vmSaveChangeCardInfoFeedBack(sArray);
            if (returnString.Contains("resultCode=0"))
            {
                BL_PAYMENT.vmUpdateSchpaymentStatus(mSchPayId, orderRef);
                // Add by Chester 2018.10.29
                // Send the email after canceling the auto pay successfully
                Helper_Email.SendEmailAfterCancelingAutoPaySuccess(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID);
                // End
                return Ok(0);
            }
            else
            {
                // Add by Chester 2018.10.29
                // Send the email after canceling the auto pay unsuccessfully

                //Helper_Email.SendEmailAfterCancelingAutoPaySuccess(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID);

                Helper_Email.SendEmailAfterCancelingAutoPayFailure(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID);

                // End
                return Ok(-1);
            }
        }
    }
}
