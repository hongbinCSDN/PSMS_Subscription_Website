using PSMS_Sub_DM;
using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace PSMS_Sub_DA
{
  public  class RESETPASSWORD_DA
    {

        private ReadCSVHelper _ReadCSV;
        protected ReadCSVHelper ToReadCSV
        {
            get { return _ReadCSV ?? (_ReadCSV = new ReadCSVHelper()); }
        }

        private SendEmailHelper _SendEmailHelper;

        protected SendEmailHelper sendemail
        {
            get { return _SendEmailHelper ?? (_SendEmailHelper = new SendEmailHelper()); }
        }


        //Add by Haskin 2018.07.24
     
        public string SendEmailByPersonalInfo(CUSTOMER customer, string Language_Value)
        {
            string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] sqlParams = new SqlParameter[]
             {
                    new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
             };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return "false";
            }
            else
            {
                string strCode = "";
                customer.EMAIL = ds.Tables[0].Rows[0]["EMAIL"].ToString();
                SendEmailHelper sendEmail = new SendEmailHelper();
                strCode = sendEmail.getCode();             
                sendemail.sendResetPasswordEmial(customer.CUSTOMER_ID, customer.EMAIL, strCode, Language_Value);
                return strCode;
            }

        }
        //Add by Haskin 2018.07.24
        public int ResetPassword(CUSTOMER customer)
        {
            string sSql = "SP_ResetPassword";
            SqlParameter[] updatesqlParams = new SqlParameter[]
            {
                        new SqlParameter("@PASSWORD",customer.PASSWORD),
                        new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID)
            };
            int variation = DBHelper.Exec(sSql,updatesqlParams,CommandType.StoredProcedure);
            return variation;
        }
        //
        //Add by Haskin 2018.07.25
        public CUSTOMER QueryEmailByCustomerID(CUSTOMER customer)
        {
            string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] sqlParams = new SqlParameter[]
             {
                    new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
             };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            if (ds.Tables[0].Rows.Count == 0)
            {

                return new CUSTOMER() { EMAIL = "Failure" };
            }
            else
            {
                return new CUSTOMER() { CUSTOMER_ID = ds.Tables[0].Rows[0]["CUSTOMER_ID"].ToString(), EMAIL = ds.Tables[0].Rows[0]["EMAIL"].ToString() };
            }

        }
        // Add by Haskin 2018.07.26
        public CUSTOMER QueryEmailByCustomerEmail(CUSTOMER customer)
        {
            string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE EMAIL=@EMAIL";
            SqlParameter[] sqlParams = new SqlParameter[]
             {
                    new SqlParameter("@EMAIL",customer.EMAIL),
             };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            if (ds.Tables[0].Rows.Count == 0)
            {

                return new CUSTOMER() { EMAIL = "Failure" };

            }
            else
            {

                return new CUSTOMER() { CUSTOMER_ID = ds.Tables[0].Rows[0]["CUSTOMER_ID"].ToString(), EMAIL = ds.Tables[0].Rows[0]["EMAIL"].ToString() };
            }
        }







    }
}
