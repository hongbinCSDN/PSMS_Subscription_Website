using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_Utility;
using PSMS_VM.Subscription;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.Subcription
{
    [WebApiTracker, Authenticate]
    public class SubscriptionController : ApiController
    {
        private SUBSCRIPTION_BL _SUBSCRIPTION_BL;
        protected SUBSCRIPTION_BL BL_SUBSCRIPTION
        {
            get { return _SUBSCRIPTION_BL ?? (_SUBSCRIPTION_BL = new SUBSCRIPTION_BL()); }
        }

        private EmailVerifyCodeSessionVM _CANCELAUTOPAY_EMAIL_VERIFYSESSION;
        protected EmailVerifyCodeSessionVM CANCELAUTOPAY_EMAIL_VERIFYSESSION
        {
            get
            {
                return _CANCELAUTOPAY_EMAIL_VERIFYSESSION ?? (_CANCELAUTOPAY_EMAIL_VERIFYSESSION = new EmailVerifyCodeSessionVM());
            }
        }

        // Modify  by Chester 2018.11.03
        private SendEmailHelper _SendEmailHelper_Utility;
        protected SendEmailHelper Utility_SendEmailHelper
        {
            get
            {
                return _SendEmailHelper_Utility ?? (_SendEmailHelper_Utility = new SendEmailHelper());
            }
        }

        private CUSTOMER_BL _CUSTOMER_BL;
        protected CUSTOMER_BL BL_CUSTOMER
        {
            get
            {
                return _CUSTOMER_BL ?? (_CUSTOMER_BL = new CUSTOMER_BL());
            }
        }

        private CUSTOMER _CUSTOMER;
        protected CUSTOMER customer
        {
            get { return _CUSTOMER ?? (_CUSTOMER = new CUSTOMER()); }
        }

        private SALES_BL _SALES_BL;
        protected SALES_BL BL_SALES
        {
            get
            {
                return _SALES_BL ?? (_SALES_BL = new SALES_BL());
            }
        }
        // Modify end



        [Route("System/GetCustomerSubscriptions")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetCustomerProducts()
        {
            return await Task.FromResult(GetCustomerSubscriptions());
        }

        public IHttpActionResult GetCustomerSubscriptions()
        {
            try
            {
                DataSet ds = BL_SUBSCRIPTION.vmQueryByCustomerSubscriptions(SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID);
                Dictionary<string, SubscriptionListVM> result = new Dictionary<string, SubscriptionListVM>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SubscriptionListVM subscriptionListVM = new SubscriptionListVM();
                    subscriptionListVM.RECORD_ID = dr["PAYMENT_RECORD_ID"].ToString();
                    subscriptionListVM.ORDERREF = dr["ORDERREF"].ToString();
                    subscriptionListVM.PRODUCT_CR_TIME = dr["PRODUCT_CR_TIME"].ToString();
                    subscriptionListVM.PRODUCT_AT_TIME = dr["PRODUCT_AT_TIME"].ToString();
                    subscriptionListVM.DOMAIN_NAME = dr["DOMAIN_NAME"].ToString();
                    subscriptionListVM.CREATE_TIME = dr["CREATE_TIME"].ToString();
                    subscriptionListVM.PAYMENT_TYPE_ID = dr["PAYMENT_TYPE_ID"].ToString();   //Add by bill 2018.9.3
                    subscriptionListVM.PAYWAY_ID = dr["PAYWAY_ID"].ToString();
                    if (dr["STATUS_CODE"].ToString() != BL_SUBSCRIPTION.GetAlicloudstatusBySequence("140")&&dr["STATUS_CODE"].ToString() != BL_SUBSCRIPTION.GetAlicloudstatusBySequence("RN|140"))
                    {
                        subscriptionListVM.STATUS = BL_SUBSCRIPTION.GetVMStatusByStatusID("1");
                    }
                    else
                    {
                        if (result.ContainsKey(subscriptionListVM.RECORD_ID))
                        {
                            subscriptionListVM.STATUS = BL_SUBSCRIPTION.GetVMStatusByStatusID("4");
                        }
                        else
                        {
                            subscriptionListVM.STATUS = dr["VM_STATUS"].ToString();
                        }
                    }


                    if (result.ContainsKey(subscriptionListVM.RECORD_ID))
                    {
                        result[subscriptionListVM.RECORD_ID].STATUS = subscriptionListVM.STATUS;
                    }
                    else
                    {
                        result.Add(subscriptionListVM.RECORD_ID, subscriptionListVM);
                    }
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/GetSubscriptionDetail")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetSubscriptionDetail(string orderref)
        {
            return await Task.FromResult(GetSubscriptionDetail(orderref));
        }

        public IHttpActionResult GetSubscriptionDetail(string orderref)
        {
            try
            {
                DataSet subscription = BL_SUBSCRIPTION.vmQueryBySubscriptionDetail(SessionWrapper.CurrentUser.CustomerID, orderref, CacheWrapper.CurrentMultilingual.MultilingualID);
                return Ok(subscription);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/CheckSubscriptionStatus")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetSubscriptionStatus(string orderref)
        {
            return await Task.FromResult(CheckSubscriptionStatus(orderref));
        }

        public IHttpActionResult CheckSubscriptionStatus(string orderref)
        {
            try
            {
                DataSet ds = BL_SUBSCRIPTION.vmQueryBySubscriptionStatus(SessionWrapper.CurrentUser.CustomerID, orderref);
                List<string> status = new List<string>();
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    if (dr["STATUS_CODE"].ToString() != BL_SUBSCRIPTION.GetAlicloudstatusBySequence("140"))
                //    {
                //        status.Add(BL_SUBSCRIPTION.GetVMStatusByStatusID("1"));
                //    }
                //    else
                //    {
                //        status.Add(dr["VM_STATUS"].ToString());
                //    }
                //}
                
                // Determine if ECS exist
                if (ds.Tables[0].Rows.Count > 0)
                {
                    status.Add(ds.Tables[0].Rows[0]["VM_STATUS"].ToString());
                }
                else
                {
                    status.Add("");
                }
                // Determine if RDS and SLB exist
                if (ds.Tables[1].Rows.Count > 0)
                {
                    status.Add(ds.Tables[1].Rows[0]["SLB_STATUS"].ToString());
                    status.Add(ds.Tables[3].Rows[0]["RDS_STATUS"].ToString());
                }
                else
                {
                    status.Add("");
                    if (ds.Tables[3].Rows[0]["RDS_STATUS"].ToString()=="Abnormal")
                    {
                        status.Add("");
                    }
                }

                // Determine if DNS exist
                if (ds.Tables[2].Rows.Count > 0)
                {
                    status.Add(ds.Tables[2].Rows[0]["DNS_STATUS"].ToString());
                }
                else
                {
                    status.Add("");
                }


                return Ok(status);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        // Add by Chester 2018-10-26
        [Route("System/GetCancelAutoPayVerifyEmail")]
        [HttpGet]
        public Task<IHttpActionResult> pSendVerifyCodeEmail()
        {
            return Task.FromResult(SendVerifyCodeEmail());
        }
        public IHttpActionResult SendVerifyCodeEmail()
        {
            try
            {
                string verifyCode = Utility_SendEmailHelper.getCode();

                CANCELAUTOPAY_EMAIL_VERIFYSESSION.EmailVerifyCode = verifyCode;
                CANCELAUTOPAY_EMAIL_VERIFYSESSION.ExpiredTime = DateTime.Now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["AccountVaildCodeEmailExpireDate"].ToString().Trim()));
                SessionWrapper.CancelAutoPayVerifyCode = CANCELAUTOPAY_EMAIL_VERIFYSESSION;

                Utility_SendEmailHelper.SendModifyPasswordVerifyCodeEmail(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, verifyCode, CacheWrapper.CurrentMultilingual.MultilingualID);
                return Ok("1");
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/VerifyEmailCode")]
        [HttpPost]
        public async Task<IHttpActionResult> pVerifyCancelAutoPayCode([FromBody]string verifyCode)
        {
            return await Task.FromResult(VerifyCancelAutoPayCode(verifyCode));
        }

        public IHttpActionResult VerifyCancelAutoPayCode(string verifyCode)
        {
            try
            {
                if (SessionWrapper.CancelAutoPayVerifyCode == null || DateTime.Now >= SessionWrapper.CancelAutoPayVerifyCode.ExpiredTime)
                {
                    SessionWrapper.CancelAutoPayVerifyCode = null;
                    return Ok(-1);
                }
                if (verifyCode == SessionWrapper.CancelAutoPayVerifyCode.EmailVerifyCode)
                {
                    SessionWrapper.CancelAutoPayVerifyCode = null;
                    return Ok(1);
                }
                else
                {
                    return Ok(0);
                }
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        [Route("System/ContactASL")]
        [HttpGet]
        public async Task<IHttpActionResult> pContactASL()
        {
            return await Task.FromResult<IHttpActionResult>(ContactASL());
        }

        public IHttpActionResult ContactASL()
        {
            try
            {
                Utility_SendEmailHelper.SendEmailToContactCustomer(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID);

                customer.CUSTOMER_ID = SessionWrapper.CurrentUser.CustomerID;
                DataSet customer_info = BL_CUSTOMER.vmQueryByDataSetGetPersionInfo(customer);
                string sales = BL_SALES.vmQuerySales().Tables[0].Rows[0][1].ToString();
                Utility_SendEmailHelper.SendEmailToContactSellers(sales, SessionWrapper.CurrentUser.CustomerID, customer_info, CacheWrapper.CurrentMultilingual.MultilingualID);
                return Ok("1");
            }
            catch (Exception e)
            {
                return Ok(e);
            }


        }
        // End
    }
}
