using PSMS_Sub_BL;
using PSMS_Sub_WebAPI.Common;
using PSMS_VM.Subscription;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.Subcription
{
    [WebApiTracker,Authenticate]
    public class SubscriptionController : ApiController
    {
        private SUBSCRIPTION_BL _SUBSCRIPTION_BL;
        protected SUBSCRIPTION_BL BL_SUBSCRIPTION
        {
            get { return _SUBSCRIPTION_BL ?? (_SUBSCRIPTION_BL = new SUBSCRIPTION_BL()); }
        }

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
                    if (dr["STATUS_CODE"].ToString() != BL_SUBSCRIPTION.GetAlicloudstatusBySequence("140"))
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
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["STATUS_CODE"].ToString() != BL_SUBSCRIPTION.GetAlicloudstatusBySequence("140"))
                    {
                        status.Add(BL_SUBSCRIPTION.GetVMStatusByStatusID("1"));
                    }
                    else
                    {
                        status.Add(dr["VM_STATUS"].ToString());
                    }
                }
                return Ok(status);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
}
