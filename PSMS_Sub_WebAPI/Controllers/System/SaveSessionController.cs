using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    public class SaveSessionController : ApiController
    {

        private TOKEN_BL _TOKEN_BL;
        protected TOKEN_BL BL_TOKEN
        {
            get { return _TOKEN_BL ?? (_TOKEN_BL = new TOKEN_BL()); }
        }

        /// <summary>
        /// Add by bill 2018-9-17
        /// </summary>
        private CustomerVM _CustomerVM;
        protected CustomerVM customerVM
        {
            get { return _CustomerVM ?? (_CustomerVM = new CustomerVM()); }
        }


        //[Route("System/SaveSession")]
        //[HttpPost]
        //public IHttpActionResult Session_Save(CustomerVM session)
        //{

        //    SessionVM v = new SessionVM() { TokenID = session.TOKEN, CustomerID = session.CUSTOMERID };
        //    SessionWrapper.CurrentUser = v;
        //    BL_TOKEN.SaveTokenToDB(session);  
        //    return Ok("Save Success !");
        //}

        [Route("System/GetToken")]
        [HttpGet]
        public IHttpActionResult GetTokenInSession()
        {
            try
            {
                customerVM.TOKEN = SessionWrapper.CurrentUser.TokenID;
                customerVM.EMAIL = SessionWrapper.CurrentUser.Email;
                customerVM.LOGINTIME = SessionWrapper.CurrentUser.LoginTime;
                customerVM.CUSTOMERID = SessionWrapper.CurrentUser.CustomerID;
                return Ok(customerVM);
                //string token = SessionWrapper.CurrentUser.TokenID;
                //return Ok(token);
            }
            catch(Exception e)
            {
                //string IsHaveToken = null;  
                //return Ok(IsHaveToken);
                return Ok(customerVM);
            }
            
        }

        [Route("System/RemoveToken")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult RemoveToken()
        {
            try
            {
                SessionWrapper.ClearAll();
                return Ok();
            }
            catch(Exception e)
            {
                return Ok(e);
            }
        }
       
    }

    
}
