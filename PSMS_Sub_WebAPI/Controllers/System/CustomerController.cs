using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace PSMS_Sub_WebAPI.Controllers.System
{
    [WebApiTracker,Authenticate]
    public class CustomerController : ApiController
    {
        private CUSTOMER_BL _CUSTOMER_BL;
        protected CUSTOMER_BL BL_CUSTOMER
        {
            get { return _CUSTOMER_BL ?? (_CUSTOMER_BL = new CUSTOMER_BL()); }
        }

        private CUSTOMER _CUSTOMER;
        protected CUSTOMER customer
        {
            get { return _CUSTOMER ?? (_CUSTOMER = new CUSTOMER()); }
        }


        /// <summary>
        /// Get customer's personal information
        /// </summary>
        /// <returns></returns>
        [Route("System/GetCustomerPersonalInfo")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetCustomerPersonalInfo()
        {
            return await Task.FromResult(GetCustomerPersonalInfo());
        }

        public IHttpActionResult GetCustomerPersonalInfo()
        {
            try
            {
                //CUSTOMER customer = new CUSTOMER();
                customer.CUSTOMER_ID = SessionWrapper.CurrentUser.CustomerID;
                return Ok(BL_CUSTOMER.vmQueryByGetPersonalInfo(customer));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        //Modify / Add by Chester 2018.07.25

        /// <summary>
        /// Update The Customer's Personal Information
        /// </summary>
        /// <param name="customer">Update Customer</param>
        /// <returns>Update Result</returns>
        [Route("System/UpdateCustomerPersonalInfo")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> pUpdateCustomerPersonalInfo(CUSTOMER customer)
        {
            return await Task.FromResult(UpdateCustomerPersonalInfo(customer));
        }
        
        public IHttpActionResult UpdateCustomerPersonalInfo(CUSTOMER customer)
        {
            try
            {
                customer.CUSTOMER_ID = SessionWrapper.CurrentUser.CustomerID;  //Modify / Add by Chester 2018.08.10
                ResultVM result = BL_CUSTOMER.vmQueryByUpdatePersonalInfo(customer);
                if (Convert.ToInt32(result.Data) == 1)
                {
                    SessionWrapper.CurrentUser.Email = customer.EMAIL;
                }
                return Ok(result);
            }catch(Exception e)
            {
                return Ok(e);
            }
        }

        //Modify end
    }
}
