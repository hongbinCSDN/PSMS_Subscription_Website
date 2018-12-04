using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DA
{
    public class SUBSCRIPTION_DA
    {
        /// <summary>
        /// Get The Subscription Plan Detail By orderref and customer_id
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <param name="multilingual"></param>
        /// <returns></returns>
        public DataSet GetQueryBySubscriptionDetail(string customer_id, string orderref, string multilingual)
        {
            string sSql = "SP_Prod_SubscriptionDetail";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@USERNAME",customer_id),
                new SqlParameter("@ORDERREF",orderref),
                new SqlParameter("@MULTILINGUAL",multilingual)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return ds;
        }

        /// <summary>
        /// Get The Current Customer's All Subscription Plans
        /// </summary>
        /// <param name="customer_id"></param>
        /// <returns></returns>
        public DataSet GetQueryByCustomerSubscriptions(string customer_id, string multilingual)
        {
            string sSql = "SP_Prod_SubscriptionList";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@USERNAME",customer_id)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return ds;
        }

        public DataSet GetQueryBySubscriptionStatus(string user,string orderref)
        {
            string sSql = "SP_Prod_QueryCloudResourceStatus";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_ID",user),
                new SqlParameter("@ORDERREF",orderref)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return ds;
        }

        /// <summary>
        /// Get AliCloud Status By Status Sequence
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public string GetAlicloudstatusBySequence(string sequence)
        {
            string sSql = "SELECT * FROM [dbo].[SYS_T_ALICLOUDSTATUS]  WHERE SEQUENCE=@SEQUENCE";
            SqlParameter[] sqlParam = new SqlParameter[]{
                new SqlParameter("@SEQUENCE",sequence)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParam);
            return ds.Tables[0].Rows[0]["STATUS_CODE"].ToString();
        }

        /// <summary>
        /// Get The VM Status By StatusID
        /// </summary>
        /// <param name="status_id"></param>
        /// <returns></returns>
        public string GetVMStatusByStatusID(string status_id)
        {
            string sSql = "SELECT * FROM [dbo].SUS_T_VM_STATUS  WHERE STATUS_ID=@STATUS_ID";
            SqlParameter[] sqlParam = new SqlParameter[]{
                new SqlParameter("@STATUS_ID",status_id)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParam);
            return ds.Tables[0].Rows[0]["STATUS_NAME"].ToString();
        }
    }
}
