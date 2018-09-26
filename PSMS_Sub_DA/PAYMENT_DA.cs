using PSMS_Sub_DM;
using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DA
{
    public class PAYMENT_DA
    {

        private SendEmailHelper _SendEmailHelper;

        protected SendEmailHelper sendemail
        {
            get { return _SendEmailHelper ?? (_SendEmailHelper = new SendEmailHelper()); }
        }

        /// <summary>
        /// Add payment records
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddPaymentRecoerd(PAYMENTRECORD data, string Multingual_ID)
        {
            string sSql = "SP_Payment_AddPaymentRecord";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@PRC",data.PRC),
                new SqlParameter("@SRC",data.SRC),
                new SqlParameter("@ORD",data.ORD),
                new SqlParameter("@REF",data.REF),
                new SqlParameter("@PAYREF",data.PAYREF),
                new SqlParameter("@SUCCESSCODE",data.SUCCESSCODE),
                new SqlParameter("@AMT",data.AMT),
                new SqlParameter("@CUR",data.CUR),
                new SqlParameter("@HOLDER",data.HOLDER),
                new SqlParameter("@AUTHID",data.AUTHID),
                new SqlParameter("@ALERTCODE",data.ALERTCODE),
                new SqlParameter("@REMARK",data.REMARK),
                new SqlParameter("@ECI",data.ECI),
                new SqlParameter("@PAYERAUTH",data.PAYERAUTH),
                new SqlParameter("@SOURCEIP",data.SOURCEIP),
                new SqlParameter("@IPCOUNTRY",data.IPCOUNTRY),
                new SqlParameter("@PAYMETHOD",data.PAYMETHOD),
                new SqlParameter("@CARDLSSUINGCOUNTRY",data.CARDLSSUINGCOUNTRY),
                new SqlParameter("@CHANNELTYPE",data.CHANNELTYPE),
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            string UpdateSql = "SP_Payment_UpdatePaymentStatus";
            SqlParameter[] UpdatesqlParams = new SqlParameter[]
           {
               new SqlParameter("@REF",data.REF),
               new SqlParameter("@AMT",data.AMT),
               new SqlParameter("@SUCCESSCODE",data.SUCCESSCODE)
           };
            DataSet Updateds = DBHelper.GetDataSet(UpdateSql, UpdatesqlParams, CommandType.StoredProcedure);
            //Add by bill 2018.8.30
            if (data.SUCCESSCODE == 0 && Updateds.Tables[0].Rows[0]["RESULT"].ToString() == "1")
            {
                string ProdSql = "SP_Payment_SelectProductInfo";
                SqlParameter[] SelectSqlParams = new SqlParameter[]
                {
                    new SqlParameter("@REF",data.REF),
                    new SqlParameter("@MULTILINGUAL_ID",Multingual_ID)
                };
                DataSet ProdInfo = DBHelper.GetDataSet(ProdSql, SelectSqlParams, CommandType.StoredProcedure);
                sendemail.SendFinishSubscribeEmail(ProdInfo, Multingual_ID);
            }
            //End
            //Add by bill 2018.8.30
            if (Updateds.Tables[0].Rows[0]["RESULT"].ToString() == "0")
            {
                //Send an email informing users that the money paid does not match the price
            }
            return int.Parse(Updateds.Tables[0].Rows[0]["RESULT"].ToString());
        }

        /// <summary>
        /// Query the value of the payment form
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public DataSet SeletProductPostForm(string category)
        {
            string sSql = "SP_Payment_SelectProductPostForm";
            SqlParameter[] sqlParams = new SqlParameter[]
           {
               new SqlParameter("@CATEGORY",category),
           };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[0]["ORDERREF"] = DateTime.Now.Ticks.ToString();  //Add by bill 2018.8.17
            }
            return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }

        public int CreateOrder(PAYMENT payment)
        {
            //string Sql1 = "SELECT * FROM SUS_T_PAYMENT WHERE ORDERREF=@ORDERREF AND CUSTOMER_ID=@CUSTOMER_ID ";
            //SqlParameter[] sqlParams1 = new SqlParameter[]
            //{
            //   new SqlParameter("@ORDERREF",payment.ORDERREF),
            //   new SqlParameter("@CUSTOMER_ID",payment.CUSTOMER_ID)
            //};
            //DataSet ds1 = DBHelper.GetDataSet(Sql1, sqlParams1);
            //if(ds1.Tables[0].Rows.Count > 0)
            //{
            //    return 0;
            //}
            //else
            //{
            string sSql = "SP_Payment_CreateOrder";
            payment.STATUS_CODE = "NULL";
            if (payment.RENEWAL_LAST_ORDERREF == null || payment.RENEWAL_LAST_ORDERREF == "")
            {
                payment.RENEWAL_LAST_ORDERREF = "N";              
            }          
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ORDERREF",payment.ORDERREF),
                new SqlParameter("@CUSTOMER_ID",payment.CUSTOMER_ID),
                new SqlParameter("@STATUS_CODE",payment.STATUS_CODE),
                new SqlParameter("@AMOUNT",payment.AMOUNT),
                new SqlParameter("@PAYMENT_TYPE_ID",payment.PAYMENT_TYPE_ID),
                new SqlParameter("@DOMAIN_NAME",payment.DOMAIN_NAME),
                new SqlParameter("@RENEWAL_LAST_ORDERREF",payment.RENEWAL_LAST_ORDERREF),
                new SqlParameter("@MULTILINGUAL_ID",payment.MULTILINGUAL_ID)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return 1;
            //}
        }

        public int CheckIsCanRenew(string orederref)
        {
            int result = 0;
            string checkSql = "SP_Payment_CheckIsCanRenew";
            SqlParameter[] checkParams = new SqlParameter[]
            {
                        new SqlParameter("@ORDERREF",orederref)
            };
            DataSet checkds = DBHelper.GetDataSet(checkSql, checkParams, CommandType.StoredProcedure);
            if (checkds.Tables.Count > 0)
            {
                if (checkds.Tables[0].Rows[0]["RESULT"].ToString() == "1")
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public string CheckDomainName(string domain_name)
        {
            string sSql1 = "SELECT * FROM SYS_T_BLACKLISTDOMAIN WHERE BLACK_DOMAIN_NAME=@BLACK_DOMAIN_NAME";
            SqlParameter[] sqlParams1 = new SqlParameter[]
            {
               new SqlParameter("@BLACK_DOMAIN_NAME",domain_name)
            };
            DataSet ds1 = DBHelper.GetDataSet(sSql1, sqlParams1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                return ds1.Tables[0].Rows[0]["BLACK_DOMAIN_NAME"].ToString();
            }
            else
            {
                string sSql2 = "SELECT * FROM SUS_T_PAYMENT WHERE DOMAIN_NAME=@DOMAIN_NAME AND STATUS_PAYMENT = '0'"; //Modify by bill 2018.8.17
                SqlParameter[] sqlParams2 = new SqlParameter[]
                {
                    new SqlParameter("@DOMAIN_NAME",domain_name)
                };
                DataSet ds2 = DBHelper.GetDataSet(sSql2, sqlParams2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
        }

        public string UpdateStatusCode(PaymentStatusCode data)
        {
            int count = 0;
            if (data.USERNAME == ConfigurationManager.AppSettings["ExternalInterfaceAccount"].ToString().Trim() && data.PASSWORD == ConfigurationManager.AppSettings["ExternalInterfacePassword"].ToString().Trim())
            {
                string sSql = "UPDATE SUS_T_PAYMENT SET STATUS_CODE=@STATUS_CODE WHERE RECORD_ID=@RECORD_ID AND ORDERREF=@ORDERREF AND STATUS_PAYMENT='0'";
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@STATUS_CODE",data.STATUS_CODE),
                    new SqlParameter("@RECORD_ID",data.RECORD_ID),
                    new SqlParameter("@ORDERREF",data.ORDERREF)
                };
                count = DBHelper.Exec(sSql, sqlParams);
            }
            if (count > 0)
            {
                return "success";
            }
            else
            {
                return "fasle";
            }
        }

        public int UpdateOrderExpireDate(OrderExpireDateVM data)
        {
            int count = 0;
            if (data.USERNAME == ConfigurationManager.AppSettings["ExternalInterfaceAccount"].ToString().Trim() && data.PASSWORD == ConfigurationManager.AppSettings["ExternalInterfacePassword"].ToString().Trim())
            {
                string sSql = "UPDATE SUS_T_PAYMENT SET PRODUCT_CR_TIME=@PRODUCT_CR_TIME,PRODUCT_AT_TIME=@PRODUCT_AT_TIME WHERE RECORD_ID=@RECORD_ID AND ORDERREF=@ORDERREF AND STATUS_PAYMENT='0'";
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@PRODUCT_CR_TIME",data.PRODUCT_CR_TIME),
                    new SqlParameter("@PRODUCT_AT_TIME",data.PRODUCT_AT_TIME),
                    new SqlParameter("@RECORD_ID",data.RECORD_ID),
                    new SqlParameter("@ORDERREF",data.ORDERREF)
                };
                count = DBHelper.Exec(sSql, sqlParams);
            }
            if (count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
