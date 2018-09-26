using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace PSMS_Sub_WebAPI.Controllers.System
{
   
    public class LoginController : ApiController
    {

        private CUSTOMER_BL _CUSTOMER_BL;
        protected CUSTOMER_BL BL_CUSTOMER
        {
            get { return _CUSTOMER_BL ?? (_CUSTOMER_BL = new CUSTOMER_BL()); }
        }

        private PASSWORD_BL _PASSWORD_BL;
        protected PASSWORD_BL BL_PASSWORD
        {
            get { return _PASSWORD_BL ?? (_PASSWORD_BL = new PASSWORD_BL()); }
        }

        private TOKEN_BL _TOKEN_BL;
        protected TOKEN_BL BL_TOKEN
        {
            get { return _TOKEN_BL ?? (_TOKEN_BL = new TOKEN_BL()); }
        }




        /// <summary>
        /// Login Vaild
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [Route("System/Login")]
        [HttpPost]
        public async Task<IHttpActionResult> pLogin(CUSTOMER customer)
        {            
            return await Task.FromResult(Login(customer));                    
        }

        public IHttpActionResult Login(CUSTOMER customer)
        {
            try
            {               
                customer.PASSWORD = BL_PASSWORD.MD5Encrypt64(customer.PASSWORD);
                var data = BL_CUSTOMER.vmQueryByLogin(customer);
                if (data.Message == "0")
                {                   
                    return Ok("0");
                }
                else if (data.Message == "-1")
                {
                    return Ok("-1");
                }
                else if (data.Message == "-2")
                {
                    return Ok("-2");
                }
                else
                {
                    //Add by bill 2018.8.23                           
                    string url = ConfigurationManager.AppSettings["WebSiteUrl"].ToString().Trim() + "/GetToken";
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "post";
                    string postData = "grant_type=password&username=" + customer.CUSTOMER_ID + "&password=" + customer.PASSWORD;
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    string[] sArray = responseFromServer.Split(new string[] { "access_token\":\"", "\",\"token_type" }, StringSplitOptions.RemoveEmptyEntries);              
                    CustomerVM session = new CustomerVM { TOKEN = sArray[1], CUSTOMERID = customer.CUSTOMER_ID };               
                    //SessionVM v = new SessionVM() { TokenID = session.TOKEN, CustomerID = session.CUSTOMERID };
                    SessionVM v = new SessionVM() { TokenID = session.TOKEN, CustomerID = session.CUSTOMERID , Email = BL_CUSTOMER.vmQueryByDataSetGetPersionInfo(customer).Tables[0].Rows[0]["EMAIL"].ToString() , LoginTime = DateTime.Now.ToString() };
                    SessionWrapper.CurrentUser = v;
                    BL_TOKEN.SaveTokenToDB(session);
                    //End
                    //Write the login record in the SYS_T_LOGIN_LOG              
                    BL_CUSTOMER.vmQueryByLoginLog(customer.CUSTOMER_ID,GetAccessClientIP.GetClientIp());  //Add by bill 2018.8.21
                    if(customer.CUSTOMER_ID == ConfigurationManager.AppSettings["ExternalInterfaceAccount"].ToString().Trim())
                    {
                        return Ok(sArray[1]);
                    }
                    else
                    {
                        return Ok("1");
                    }
                        
                }
                
            }
            catch(Exception e)
            {
                return Ok(e);
            }
        }
    }
}
