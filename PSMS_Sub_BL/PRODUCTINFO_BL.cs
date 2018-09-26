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
                return _PRODUCTINFO_DA ?? (_PRODUCTINFO_DA=new PRODUCTINFO_DA());
            }
        }

        public ResultVM vmQueryProuctInfoByUsername(string username,string multilingual)
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
            return new ResultVM { Data = DA.GetQueryByProductInfo(customer_id, orderref,multilingual) };
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
    }
}
