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
    public class PRODUCTINFO_DA
    {

        /// <summary>
        /// Get The last Subscription Product By Username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="multilingual"></param>
        /// <returns></returns>
        public DataSet GetProductByUsername(string username, string multilingual)
        {
            string sSql = "SP_Prod_SubcriptedProduct";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@USERNAME",username),
                new SqlParameter("@MULTILINGUAL",multilingual)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return ds;
        }

        // Modify By Chester
        //public string GetCheckStatusByUsername(string username)
        //{
        //    string sSql = "SELECT TOP 1 * FROM SUS_T_PAYMENT WHERE CUSTOMER_ID=@CUSTOMER_ID AND STATUS_PAYMENT='0' ORDER BY CREATE_TIME DESC";
        //    SqlParameter[] sqlParams = new SqlParameter[]
        //    {
        //        new SqlParameter("@CUSTOMER_ID",username)
        //    };
        //    DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
        //    string status = ds.Tables[0].Rows[0]["STATUS_CODE"].ToString();
        //    return status;
        //}
        //Modify end

        /// <summary>
        /// Select ProductInfo By Customer_ID and Orderref
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        public DataSet GetQueryByProductInfo(string customer_id, string orderref, string multilingual)
        {
            string sSql = "SELECT PAYMENT_TYPE_ID FROM SUS_T_PAYMENT WHERE ORDERREF=@ORDERREF AND CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ORDERREF",orderref),
                new SqlParameter("@CUSTOMER_ID",customer_id)
            };
            int productTypeID = Convert.ToInt32(DBHelper.GetDataSet(sSql, sqlParams).Tables[0].Rows[0]["PAYMENT_TYPE_ID"]);
            string productSql = "SELECT * FROM PAY_T_PRODUCTINFO WHERE PRODUCT_ID=@PRODUCT_ID AND MULTILINGUAL_ID=@MULTILINGUAL";
            SqlParameter[] productSqlParams = new SqlParameter[]
            {
                new SqlParameter("@PRODUCT_ID",productTypeID),
                new SqlParameter("@MULTILINGUAL",multilingual)
            };
            DataSet productInfo = DBHelper.GetDataSet(productSql, productSqlParams);
            return productInfo;
        }

        /// <summary>
        /// Get The Product'Status Percentage After Payment
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        public string GetQueryByStatusPercent(string customer_id, string orderref)
        {
            string sSql = "SELECT PERCENTAGE FROM SYS_T_ALICLOUDSTATUS WHERE STATUS_CODE=(SELECT STATUS_CODE FROM SUS_T_PAYMENT WHERE ORDERREF=@ORDERREF AND CUSTOMER_ID=@CUSTOMER_ID)";
            SqlParameter[] sqlParams = new SqlParameter[]
           {
                new SqlParameter("@CUSTOMER_ID",customer_id),
                new SqlParameter("@ORDERREF",orderref)
           };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            return ds.Tables[0].Rows[0]["PERCENTAGE"].ToString();
        }

        public string GetStatusPercentByUsername(string username)
        {
            string sSql = "SELECT PERCENTAGE FROM SYS_T_ALICLOUDSTATUS WHERE STATUS_CODE=(SELECT TOP 1 STATUS_CODE FROM SUS_T_PAYMENT WHERE CUSTOMER_ID=@CUSTOMER_ID AND STATUS_PAYMENT='0' ORDER BY CREATE_TIME DESC)";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_ID",username)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            return ds.Tables[0].Rows[0]["PERCENTAGE"].ToString();
        }

        /// <summary>
        /// Check The Payment Status
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="orderref"></param>
        /// <returns></returns>
        //public string GetQueryByCheckStatus(string customer_id, string orderref)
        //{
        //    string sSql = "SELECT * FROM SUS_T_PAYMENT WHERE CUSTOMER_ID=@CUSTOMER_ID AND ORDERREF=@ORDERREF";
        //    SqlParameter[] sqlParams = new SqlParameter[]
        //   {
        //        new SqlParameter("@CUSTOMER_ID",customer_id),
        //        new SqlParameter("@ORDERREF",orderref)
        //   };
        //    DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
        //    string status = ds.Tables[0].Rows[0]["STATUS_CODE"].ToString();
        //    return status;
        //}


    }
}
