using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.Payment
{
    [WebApiTracker,Authenticate]
    public class PaymentController : ApiController
    {
        private PAYMENT_BL _PAYMENT_BL;
        protected PAYMENT_BL BL_PAYMENT
        {
            get { return _PAYMENT_BL ?? (_PAYMENT_BL = new PAYMENT_BL()); }
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
                else if(CacheWrapper.CurrentMultilingual.MultilingualID == "3")
                {
                    ds.Tables[0].Rows[0]["LANG"] = "X";
                }
                return Ok(ds);
            }
            catch(Exception e)
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
            catch(Exception e)
            {
                return Ok(e);
            }
        }

    }
}
