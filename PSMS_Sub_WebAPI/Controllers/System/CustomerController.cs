using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_Utility;
using PSMS_VM;
using PSMS_VM.ModifyPersonalInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace PSMS_Sub_WebAPI.Controllers.System
{
    [WebApiTracker, Authenticate]
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

        private EmailVerifyCodeSessionVM _VERIFYCODESESSION;
        protected EmailVerifyCodeSessionVM VERIFYCODESESSION
        {
            get
            {
                return _VERIFYCODESESSION ?? (_VERIFYCODESESSION = new EmailVerifyCodeSessionVM());
            }
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
        public async Task<IHttpActionResult> pUpdateCustomerPersonalInfo(ModifyPersonalInfoVM customerVM)
        {
            return await Task.FromResult(UpdateCustomerPersonalInfo(customerVM));
        }

        public IHttpActionResult UpdateCustomerPersonalInfo(ModifyPersonalInfoVM customerVM)
        {
            try
            {
                CUSTOMER customer = customerVM.CUSTOMER;                       // Modify / Add by Chester 2018.09.29
                customer.CUSTOMER_ID = SessionWrapper.CurrentUser.CustomerID;  // Modify / Add by Chester 2018.08.10
                ResultVM result = new ResultVM();
                //if (customerVM.ISMODIFYEMAIL)
                //{
                //    if (SessionWrapper.ModifyPersonalInfoVerifyCode == null || DateTime.Now >= SessionWrapper.ModifyPersonalInfoVerifyCode.ExpiredTime)
                //    {
                //        SessionWrapper.ModifyPersonalInfoVerifyCode = null;
                //        result.Data = -3;
                //        return Ok(result);
                //    }
                //    if (customerVM.VERIFYCODE != SessionWrapper.ModifyPersonalInfoVerifyCode.EmailVerifyCode)
                //    {
                //        result.Data = -2;
                //        return Ok(result);
                //    }
                //}
                //else
                //{
                //    customer.EMAIL = SessionWrapper.CurrentUser.Email;
                //}
                customer.EMAIL = SessionWrapper.CurrentUser.Email;
                //result = BL_CUSTOMER.vmQueryByUpdatePersonalInfo(customer);
                //if (Convert.ToInt32(result.Data) == 1)
                //{
                //    SessionWrapper.ModifyPersonalInfoVerifyCode = null;
                //    SessionWrapper.CurrentUser.Email = customer.EMAIL;
                //}
                return Ok(BL_CUSTOMER.vmQueryByUpdatePersonalInfo(customer));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        //Modify end

        // Modify / Add by Chester 2018.09.28

        //[Route("System/TestCustomer")]
        //[HttpGet]
        //[Authorize]
        //public async Task<IHttpActionResult> pSendVerifyEmail()
        //{
        //    return await Task.FromResult(SendVerifyEmail());
        //}

        //public IHttpActionResult SendVerifyEmail()
        //{
        //    try
        //    {
        //        SendEmailHelper sendEmailHelper = new SendEmailHelper();
        //        string verifyCode = sendEmailHelper.getCode();

        //        VERIFYCODESESSION.EmailVerifyCode = verifyCode;
        //        VERIFYCODESESSION.ExpiredTime = DateTime.Now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["AccountVaildCodeEmailExpireDate"].ToString().Trim()));
        //        SessionWrapper.ModifyPersonalInfoVerifyCode = VERIFYCODESESSION;

        //        sendEmailHelper.SendModifyPasswordVerifyCodeEmail(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, verifyCode, CacheWrapper.CurrentMultilingual.MultilingualID);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e);
        //    }
        //}

        // Modify End
    }
}
