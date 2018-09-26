using PSMS_Sub_DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class SUBSCRIPTION_BL
    {
        private SUBSCRIPTION_DA _SUBSCRIPTION_DA;
        protected SUBSCRIPTION_DA DA
        {
            get { return _SUBSCRIPTION_DA ?? (_SUBSCRIPTION_DA = new SUBSCRIPTION_DA()); }
        }

        /// <summary>
        /// Get The Current Customer's All Subscription Plans
        /// </summary>
        /// <param name="customer_id"></param>
        /// <returns></returns>
        public DataSet vmQueryByCustomerSubscriptions(string customer_id,string multilingual)
        {
            return DA.GetQueryByCustomerSubscriptions(customer_id, multilingual);
        }

        /// <summary>
        /// Get The Subscription Plan Detail By Orderref and customer_id
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <param name="multilingual"></param>
        /// <returns></returns>
        public DataSet vmQueryBySubscriptionDetail(string customer_id,string orderref,string multilingual)
        {
            return DA.GetQueryBySubscriptionDetail(customer_id, orderref, multilingual);
        }

        /// <summary>
        /// Get The Subscription Status By Orderref
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        public DataSet vmQueryBySubscriptionStatus(string customer_id,string orderref)
        {
            return DA.GetQueryBySubscriptionStatus(customer_id, orderref);
            
        }

        /// <summary>
        /// Get The VM Status By StatusID
        /// </summary>
        /// <param name="status_id"></param>
        /// <returns></returns>
        public string GetVMStatusByStatusID(string status_id)
        {
            return DA.GetVMStatusByStatusID(status_id);
        }

        /// <summary>
        ///  Get AliCloud Status By Status Sequence
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public string GetAlicloudstatusBySequence(string sequence)
        {
            return DA.GetAlicloudstatusBySequence(sequence);
        }
    }
}
