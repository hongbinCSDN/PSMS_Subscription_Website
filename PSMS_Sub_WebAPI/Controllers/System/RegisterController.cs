using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    [WebApiTracker]
    public class RegisterController : ApiController
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

        /// <summary>
        /// Register Customer Account
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [Route("System/Register")]
        [HttpPost]       
        public async Task<IHttpActionResult> pRegisterAsync(CUSTOMER customer)
        {
            return await Task.FromResult(Register(customer));
        }

        public IHttpActionResult Register(CUSTOMER customer)
        {
            try
            {
                customer.PASSWORD = BL_PASSWORD.MD5Encrypt64(customer.PASSWORD);
                return Ok(BL_CUSTOMER.vmQueryByRegister(customer, CacheWrapper.CurrentMultilingual.MultilingualID));
            }
            catch(Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/ActiveAccount/{activecode}/{customerid}")]
        [HttpGet]
        public IHttpActionResult AccticeAccount(string activecode, string customerid)
        {

            CUSTOMER customer = new CUSTOMER();
            customer.ACCTIVECODE = activecode;
            customer.CUSTOMER_ID = customerid;
            //var VaildAccount = BL_CUSTOMER.vmQueryByVaildAccount(customer, CacheWrapper.CurrentMultilingual.MultilingualID);
            return Ok(BL_CUSTOMER.vmQueryByVaildAccount(customer, CacheWrapper.CurrentMultilingual.MultilingualID));
        }

        [Route("System/ActiveAccount")]
        [HttpPost]
        public IHttpActionResult AccticeAccount(ActiveAccountVM account)
        {

            CUSTOMER customer = new CUSTOMER();
            customer.ACCTIVECODE = account.ACTIVECODE;
            customer.CUSTOMER_ID = account.CUSTOMER_NAME;
            return Ok(BL_CUSTOMER.vmQueryByVaildAccount(customer, CacheWrapper.CurrentMultilingual.MultilingualID));
        }





    }
}
