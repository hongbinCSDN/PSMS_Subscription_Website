using PSMS_Sub_DA;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class PRODUCTINFO_BL
    {
        private PRODUCTINFO_DA _PRODUCTINFO_DA;
        protected PRODUCTINFO_DA DA
        {
            get
            {
                return _PRODUCTINFO_DA ?? (_PRODUCTINFO_DA = new PRODUCTINFO_DA());
            }
        }

        public ResultVM vmQueryProuctInfoByUsername(string username, string multilingual)
        {
            return new ResultVM { Data = DA.GetProductByUsername(username, multilingual) };
        }

        // Modify / Add By Chester  2018.08.08

        //public string vmQueryCheckStatusByUsername(string username)
        //{
        //    return DA.GetCheckStatusByUsername(username);
        //}

        //Modify End

        public string vmQueryGetStatusPercentByUsername(string username)
        {
            return DA.GetStatusPercentByUsername(username);
        }

        /// <summary>
        /// Get The Subscripted Product Infomation
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        public ResultVM vmQueryByProductInfo(string customer_id, string orderref, string multilingual)
        {
            return new ResultVM { Data = DA.GetQueryByProductInfo(customer_id, orderref, multilingual) };
        }
       

        /// <summary>
        /// Get The Subscription Product's Status Percent
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        public string vmQueryByStatusPercent(string customer_id, string orderref)
        {
            return DA.GetQueryByStatusPercent(customer_id, orderref);
        }

        // Modify by Chester 2018-11-14
        /// <summary>
        /// Check the order for renewal
        /// </summary>
        /// <param name="orderref"></param>
        /// <param name="username"></param>
        /// <returns>
        /// 1: renewal
        /// 0: no renewal
        /// </returns>
        public int vmQueryByCheckIsRenewal(string orderref, string username)
        {
            DataSet ds = DA.GetPaymentStatusByOrder(orderref, username);
            bool isRenewal = ds.Tables[0].Rows[0]["STATUS_CODE"].ToString().StartsWith("RN|");
            if ((ds.Tables[0].Rows[0]["STATUS_CODE"].ToString() == "RENEWAL" || isRenewal) && ds.Tables[0].Rows[0]["STATUS_PAYMENT"].ToString() == "0")
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Get the domain name by orderref and username
        /// </summary>
        /// <param name="orderref"></param>
        /// <param name="username"></param>
        /// <returns>Domain name</returns>
        public string vmQueryByGetDomainName(string orderref,string username)
        {
            return DA.GetPaymentDomainName(orderref, username);
        }

        public string vmQueryByGetLastestPayInfoByUsername(string username)
        {
            return DA.GetLastestPaymentInfoByUsername(username).Tables[0].Rows[0]["ORDERREF"].ToString();
        }
        // End
    }
}
