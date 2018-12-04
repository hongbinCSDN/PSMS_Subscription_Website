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
            string sSql = "SP_Payment_CreateOrder";
            payment.STATUS_CODE = "NULL";
            if (payment.RENEWAL_LAST_ORDERREF == null || payment.RENEWAL_LAST_ORDERREF == "")
            {
                payment.RENEWAL_LAST_ORDERREF = "N";
            }
            if(payment.PAYWAY_ID == null || payment.PAYWAY_ID == "")
            {
                payment.PAYWAY_ID = "1";
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
                new SqlParameter("@MULTILINGUAL_ID",payment.MULTILINGUAL_ID),
                new SqlParameter("@PAYWAY_ID",payment.PAYWAY_ID),   //Add by bill 2018-10-18
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return 1;
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

        public ResultVM UpdateStatusCode(PaymentStatusCode data)
        {
            try
            {
                string result = "300";
                string message = "The interface account is not correct";
                if (data.USERNAME == ConfigurationManager.AppSettings["ExternalInterfaceAccount"].ToString().Trim() && data.PASSWORD == ConfigurationManager.AppSettings["ExternalInterfacePassword"].ToString().Trim())
                {
                    string sql = "SP_ExternalInterface_UpdateStatusCode";
                    SqlParameter[] sqlParams = new SqlParameter[]
                    {
                    new SqlParameter("@RECORD_ID",data.RECORD_ID),
                    new SqlParameter("@STATUS_CODE",data.STATUS_CODE)
                    };
                    DataSet ds = DBHelper.GetDataSet(sql, sqlParams, CommandType.StoredProcedure);
                    result = ds.Tables[0].Rows[0]["RESULT"].ToString();
                }
                if (result == "0")
                {
                    message = "Update successful";
                }
                else if (result == "200")
                {
                    message = "There is no this status code, please check.";
                }
                else if (result == "100")
                {
                    message = "There is no this record, please check.";
                }
                return new ResultVM { Affected = int.Parse(result), Message = message };

            }
            catch (Exception e)
            {
                return new ResultVM { Message = e.ToString() };
            }
        }

        public ResultVM UpdateOrderExpireDate(OrderExpireDateVM data)
        {

            try
            {
                string result = "300";
                string message = "The interface account is not correct";
                if (data.USERNAME == ConfigurationManager.AppSettings["ExternalInterfaceAccount"].ToString().Trim() && data.PASSWORD == ConfigurationManager.AppSettings["ExternalInterfacePassword"].ToString().Trim())
                {
                    string sql = "SP_ExternalInterface_UpdateProductExpireDate";
                    SqlParameter[] sqlParams = new SqlParameter[]
                    {
                        new SqlParameter("@RECORD_ID",data.RECORD_ID),
                        new SqlParameter("@PRODUCT_AT_TIME",data.PRODUCT_AT_TIME)

                    };
                    DataSet ds = DBHelper.GetDataSet(sql, sqlParams, CommandType.StoredProcedure);
                    result = ds.Tables[0].Rows[0]["RESULT"].ToString();
                }
                if (result == "0")
                {
                    message = "Update successful";
                }
                else if (result == "100")
                {
                    message = "There is no this record, please check.";
                }
                return new ResultVM { Affected = int.Parse(result), Message = message };
            }
            catch (Exception e)
            {
                return new ResultVM { Message = e.ToString() };
            }

        }


        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="XmlFiledValues"></param>
        /// <returns></returns>
        public string SaveQueryPaymentStatus(string[] XmlFiledValues)
        {
            try
            {
                string sql = "SP_Payment_SavePaymentStatusFromAisapay";
                SqlParameter[] sqlParams = new SqlParameter[]
                {

                    new SqlParameter("@OrderStatus",XmlFiledValues[0]),
                    new SqlParameter("@Ref",XmlFiledValues[1]),
                    new SqlParameter("@PayRef",XmlFiledValues[2]),
                    new SqlParameter("@Amt",XmlFiledValues[3]),
                    new SqlParameter("@Cur",XmlFiledValues[4]),
                    new SqlParameter("@Prc",XmlFiledValues[5]),
                    new SqlParameter("@Src",XmlFiledValues[6]),
                    new SqlParameter("@Ord",XmlFiledValues[7]),
                    new SqlParameter("@Holder",XmlFiledValues[8]),
                    new SqlParameter("@AuthId",XmlFiledValues[9]),
                    new SqlParameter("@AlertCode",XmlFiledValues[10]),
                    new SqlParameter("@Remark",XmlFiledValues[11]),
                    new SqlParameter("@Eci",XmlFiledValues[12]),
                    new SqlParameter("@PayerAuth",XmlFiledValues[13]),
                    new SqlParameter("@SourceIp",XmlFiledValues[14]),
                    new SqlParameter("@IpCountry",XmlFiledValues[15]),
                    new SqlParameter("@PayMethod",XmlFiledValues[16]),
                    new SqlParameter("@PanFirst4",XmlFiledValues[17]),
                    new SqlParameter("@PanLast4",XmlFiledValues[18]),
                    new SqlParameter("@CardIssuingCountry",XmlFiledValues[19]),
                    new SqlParameter("@ChannelType",XmlFiledValues[20]),
                    new SqlParameter("@TxTime",XmlFiledValues[21]),
                    new SqlParameter("@SuccessCode",XmlFiledValues[22]),
                    new SqlParameter("@MSchPayId",XmlFiledValues[23]),
                    new SqlParameter("@DSchPayId",XmlFiledValues[24]),
                    new SqlParameter("@MerchantId",XmlFiledValues[25]),
                    new SqlParameter("@ErrMsg",XmlFiledValues[26]),
                    new SqlParameter("@Update_Time",System.DateTime.Now),
                };
                //nt count = DBHelper.Exec(sql, sqlParams, CommandType.StoredProcedure);
                DataSet ds = DBHelper.GetDataSet(sql, sqlParams, CommandType.StoredProcedure);
                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Rows[0]["RESULT"].ToString();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }

        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="SchXmlFiledValues"></param>
        /// <returns></returns>
        public void SaveQuerySchPaymentInfo(string[] SchXmlFiledValues)
        {
            try
            {
                string sql = "SP_Payment_SaveSchPaymentInfo";
                SqlParameter[] sqlParams = new SqlParameter[]
               {
                   new SqlParameter("@mSchPayId",SchXmlFiledValues[0]),
                   new SqlParameter("@schType",SchXmlFiledValues[1]),
                   new SqlParameter("@startDate",SchXmlFiledValues[2]),
                   new SqlParameter("@endDate",SchXmlFiledValues[3]),
                   new SqlParameter("@merRef",SchXmlFiledValues[4]),
                   new SqlParameter("@amount",SchXmlFiledValues[5]),
                   new SqlParameter("@payType",SchXmlFiledValues[6]),
                   new SqlParameter("@payMethod",SchXmlFiledValues[7]),
                   new SqlParameter("@account",SchXmlFiledValues[8]),
                   new SqlParameter("@holder",SchXmlFiledValues[9]),
                   new SqlParameter("@expiryDate",SchXmlFiledValues[10]),
                   new SqlParameter("@status",SchXmlFiledValues[11]),
                   new SqlParameter("@suspendDate",SchXmlFiledValues[12]),
                   new SqlParameter("@lastTerminateDate",SchXmlFiledValues[13]),
                   new SqlParameter("@reActivateDate",SchXmlFiledValues[14]),
                   new SqlParameter("@update_time",System.DateTime.Now),
               };
                int count = DBHelper.Exec(sql, sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="DetailSchXmlFiledValues"></param>
        /// <param name="mSchPayId"></param>
        /// <returns></returns>
        public void SaveQueryDetailSchPaymentInfo(string[] DetailSchXmlFiledValues, string mSchPayId)
        {
            try
            {
                string sql = "SP_Payment_SaveDetailSchPayment";
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@mSchPayId",mSchPayId),
                    new SqlParameter("@dSchPayId",DetailSchXmlFiledValues[0]),
                    new SqlParameter("@schType",DetailSchXmlFiledValues[1]),
                    new SqlParameter("@orderDate",DetailSchXmlFiledValues[2]),
                    new SqlParameter("@tranDate",DetailSchXmlFiledValues[3]),
                    new SqlParameter("@currency",DetailSchXmlFiledValues[4]),
                    new SqlParameter("@amount",DetailSchXmlFiledValues[5]),
                    new SqlParameter("@status",DetailSchXmlFiledValues[6]),
                    new SqlParameter("@payRef",DetailSchXmlFiledValues[7]),
                    new SqlParameter("@update_time",System.DateTime.Now),
                };
                int count = DBHelper.Exec(sql, sqlParams, CommandType.StoredProcedure);


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public DataSet QueryOrderPayRef(string orderRef)
        {
            string sql = "SELECT PAYREF FROM PAY_T_PAYMENTRECORD WHERE REF=@REF";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@REF",orderRef),
            };
            DataSet ds = DBHelper.GetDataSet(sql, sqlParams);
            return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }

        // Add by chester 2018-10-22
        public DataSet QueryAutoPayMessage(string merref)
        {
            string sSql = "SELECT [mSchPayId],[account],[payMethod],[startDate],[endDate],[holder],[expiryDate] FROM PAY_T_AISAPAY_SCHEDULEPAYMENT WHERE merRef = @merRef";
            SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@merRef",merref),
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParam);
            return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }

        public DataSet QueryDetailSchPay(string schPayId)
        {
            string sSql = "SELECT [dSchPayId],[schType],[orderDate],[tranDate],[currency],[amount],[status] FROM [PAY_T_ASIAPAY_DETAILSCHPAY] WHERE mSchPayId=@SCHPAYID ";
            SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@SCHPAYID",schPayId)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParam);
            return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }

        public DataTable GetPaymentMethod()
        {
            string sSql = "SELECT * FROM SYS_T_DICTIONARY WHERE CATEGORY='PAYMETHOD' AND STATUES=1";
            DataSet ds = DBHelper.GetDataSet(sSql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0].Copy();
            return dt;
        }
        // End

        public void SaveChangeCardInfoFeedBack(string[] feedback)
        {
            try
            {
                string sql = "SP_Payment_SaveChangeCardInfoFeedBack";
                SqlParameter[] sqlParam = new SqlParameter[]
                {
                new SqlParameter("@resultCode",feedback[0]),
                new SqlParameter("@mSchPayId",feedback[1]),
                new SqlParameter("@merchantId",feedback[2]),
                new SqlParameter("@orderRef",feedback[3]),
                new SqlParameter("@status",feedback[4]),
                new SqlParameter("@errMsg",feedback[5]),
               };
                DBHelper.Exec(sql, sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void UpdateCardInfoFeedBack(string orderRef, string mSchPayId)
        {
            try
            {
                string sql = "SP_Payment_UpdateCardInfo_feedback";
                SqlParameter[] sqlParam = new SqlParameter[]
               {
                new SqlParameter("@ORDERREF",orderRef),
                new SqlParameter("@MSCHPAYID",mSchPayId),

               };
                DBHelper.Exec(sql, sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string GetSchPaymentmSchPayId(string orderRef)
        {
            try
            {
                string sql = "SELECT mSchPayId FROM PAY_T_AISAPAY_SCHEDULEPAYMENT WHERE merRef = @orderRef";
                SqlParameter[] sqlParam = new SqlParameter[]
                {
                    new SqlParameter("@orderRef",orderRef)
                };
                DataSet ds = DBHelper.GetDataSet(sql, sqlParam);
                return ds.Tables[0].Rows.Count == 0 ? "" : ds.Tables[0].Rows[0]["mSchPayId"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateSchPaymentStatus(string mSchPayId, string orderRef)
        {
            try
            {
                string sql = "SP_Payment_UpdateSchPaymentStatus";
                SqlParameter[] sqlParam = new SqlParameter[]
                {
                    new SqlParameter("@mSchPayId",mSchPayId),
                    new SqlParameter("@orderRef",orderRef)
                };
                DBHelper.Exec(sql, sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Add by Chester 2018.10.31
        public string GetSchPaymentAccount(string orderRef)
        {
            try
            {
                string sql = "SELECT account FROM [dbo].[PAY_T_AISAPAY_SCHEDULEPAYMENT] WHERE merRef=@ORDERREF";
                SqlParameter[] sqlParam = new SqlParameter[]
                {
                new SqlParameter("@ORDERREF",orderRef)
                };
                DataSet ds = DBHelper.GetDataSet(sql, sqlParam);
                return ds.Tables[0].Rows.Count == 0 ? "" : ds.Tables[0].Rows[0]["account"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // End
    }
}
