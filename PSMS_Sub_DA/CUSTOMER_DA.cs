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


namespace PSMS_Sub_DA
{
    public class CUSTOMER_DA
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

       
       
        public string GetQueryByLogin(CUSTOMER customer)
        {
            string message = "0";
            string YSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID AND PASSWORD=@PASSWORD AND STATUS='Y'";  
            string NSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID AND PASSWORD=@PASSWORD AND STATUS='N'";
            string AccountSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID = @CUSTOMER_ID";
            SqlParameter[] AccountSqlParams = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID)
            };
            DataSet Accountds = DBHelper.GetDataSet(AccountSql, AccountSqlParams);
            if(Accountds.Tables[0].Rows.Count > 0)
            {            
                SqlParameter[] YsqlParams = new SqlParameter[]
                {
                      new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                      new SqlParameter("@PASSWORD",customer.PASSWORD)
                };
                SqlParameter[] NsqlParams = new SqlParameter[]
                {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                new SqlParameter("@PASSWORD",customer.PASSWORD)
                 };
                DataSet Yds = DBHelper.GetDataSet(YSql, YsqlParams);
                DataSet Nds = DBHelper.GetDataSet(NSql, NsqlParams);
                if (Yds.Tables[0].Rows.Count > 0)
                {
                    message = "1";
                }
                else if (Nds.Tables[0].Rows.Count > 0)
                {
                    message = "-1";
                }
                else
                    message = "-2";
                return message;
            }
            else
            {
                return "0";
            }
          //  SqlParameter[] sqlParams = new SqlParameter[]
          // {
          //      new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
          //      new SqlParameter("@PASSWORD",customer.PASSWORD)
          // };
          //  SqlParameter[] asqlParams = new SqlParameter[]
          //{
          //      new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
          //      new SqlParameter("@PASSWORD",customer.PASSWORD)
          //};
          //  DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
          //  DataSet ads = DBHelper.GetDataSet(aSql, asqlParams);
          //  if (ds.Tables[0].Rows.Count != 0)
          //  {
          //      message = "Success";
          //      return message;
          //  }
          //  if(ads.Tables[0].Rows.Count != 0 )
          //  {
          //      message = "Inactive";
          //      return message;
          //  }
          //  else
          //  {
          //      message = "False";
          //      return message;
          //  }
            //return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }


        //Modify by Hakin 20180806  Add the params 'category'
        /// <summary>
        /// Registered account
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int AddQueryByRegister(CUSTOMER customer, string category)
        {
            string checkUsernameSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] CheckUsernameSqlParams = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID)
            };
            DataSet cds = DBHelper.GetDataSet(checkUsernameSql, CheckUsernameSqlParams);
            string checkEmailSql = "SELECT * FROM SYS_T_CUSTOMER WHERE EMAIL=@EMAIL";
            SqlParameter[] CheckEmailSqlParams = new SqlParameter[]
            {
                new SqlParameter("@EMAIL",customer.EMAIL)
            };
            DataSet eds = DBHelper.GetDataSet(checkEmailSql, CheckEmailSqlParams);

            if (cds.Tables[0].Rows.Count == 0 && eds.Tables[0].Rows.Count == 0)
            {               
                string sSql = "SP_Register_Account";
                customer.CREATE_DATE = DateTime.Now.Date;
                customer.STATUS = "N";
                string activecode = Guid.NewGuid().ToString().Substring(0, 20);
                DateTime active_at_dt =DateTime.Now.AddMinutes(2);
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                new SqlParameter("@PASSWORD",customer.PASSWORD),
                new SqlParameter("@CUSTOMER_NAME",customer.CUSTOMER_NAME),
                new SqlParameter("@COMPANY",customer.COMPANY),
                new SqlParameter("@COMPANY_ADDRESS",customer.COMPANY_ADDRESS),
                new SqlParameter("@PHONE",customer.PHONE),
                new SqlParameter("@FIXED_TELEPHONE",customer.FIXED_TELEPHONE),
                new SqlParameter("@EMAIL",customer.EMAIL),
                new SqlParameter("@STATUS",customer.STATUS),
                new SqlParameter("@CNAME",customer.CNAME),
                new SqlParameter("@ACTIVECODE",activecode),
                new SqlParameter("@ACTIVECODE_AT_DT",active_at_dt),
                new SqlParameter("@CREATE_DATE",customer.CREATE_DATE),
                };
                DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);              
                sendemail.sendEmail1(customer.CUSTOMER_ID, customer.CUSTOMER_NAME, customer.EMAIL,activecode, category);     //Modify by Hakin 20180806  Add the params 'category'
                return 1;
            }
            else
            {
                if(cds.Tables[0].Rows.Count > 0 && eds.Tables[0].Rows.Count > 0)
                {
                    return -2;
                }
                else if(eds.Tables[0].Rows.Count > 0)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }             
            }    
        }
        //end
        //Modify by Hakin 20180806  Add the params 'category'
        /// <summary>
        /// Vaild Account
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool GetQueryByVaildAccount(CUSTOMER customer,string category)
        {
          //tring message = null;
          
          string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID AND ACTIVECODE=@ACTIVECODE";
          SqlParameter[] sqlParams = new SqlParameter[]
          {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                new SqlParameter("@ACTIVECODE",customer.ACCTIVECODE)
          };
          DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
          if(ds.Tables[0].Rows.Count > 0)
          {
                if((DateTime)ds.Tables[0].Rows[0]["ACTIVECODE_AT_DT"] > DateTime.Now)
                {
                    string updateSql = "UPDATE SYS_T_CUSTOMER SET STATUS='Y' WHERE CUSTOMER_ID=@CUSTOMER_ID AND ACTIVECODE=@ACTIVECODE";

                    SqlParameter[] updatesqlParams = new SqlParameter[]
                      {
                        new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                        new SqlParameter("@ACTIVECODE",customer.ACCTIVECODE)
                      };
                    DataSet updateds = DBHelper.GetDataSet(updateSql, updatesqlParams);
                    return true;
                }
                else
                {
                    
                    string activecode = Guid.NewGuid().ToString().Substring(0,20);
                    sendemail.sendEmail1(customer.CUSTOMER_ID, ds.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString(), ds.Tables[0].Rows[0]["EMAIL"].ToString(), activecode,category);
                    DateTime active_at_dt = DateTime.Now.AddMinutes(2);
                    string updateactivecode = "UPDATE SYS_T_CUSTOMER SET ACTIVECODE=@ACTIVECODE,ACTIVECODE_AT_DT=@ACTIVECODE_AT_DT WHERE CUSTOMER_ID=@CUSTOMER_ID";
                    SqlParameter[] updatesqlParams = new SqlParameter[]
                     {
                        new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
                        new SqlParameter("@ACTIVECODE",activecode),
                        new SqlParameter("@ACTIVECODE_AT_DT",active_at_dt)
                     };
                    DBHelper.GetDataSet(updateactivecode, updatesqlParams);
                    return false;
                }
                
            }
            else
            {
                return false;
            }
            
        }
        //end
        public DataSet GetQueryByGetPersonalInfo(CUSTOMER customer)
        {
            string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] sqlParams = new SqlParameter[]
             {
                    new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID),
             };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            return ds.Tables[0].Rows.Count == 0 ? null : ds;
        }

        //Modify / Add by Chester 2018.07.25

        /// <summary>
        /// Update The Customer'Personal Information
        /// </summary>
        /// <param name="customer">Update Customer</param>
        /// <returns>Update Result</returns>
        public int UpdateQueryByUpdatePersonalInfo(CUSTOMER customer)
        {
            string sSql = "SELECT * FROM SYS_T_CUSTOMER WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] CheckSqlParams = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID)
            };
            DataSet cds = DBHelper.GetDataSet(sSql, CheckSqlParams);
            string oldEmail = cds.Tables[0].Rows[0]["EMAIL"].ToString();

            if (!string.Equals(oldEmail, customer.EMAIL))
            {
                string checkOnlyEmailSql = "SELECT * FROM SYS_T_CUSTOMER WHERE EMAIL=@EMAIL";
                SqlParameter[] checkOnlyEmailSqlParams = new SqlParameter[]
                {
                new SqlParameter("@EMAIL",customer.EMAIL)
                };
                DataSet coeds = DBHelper.GetDataSet(checkOnlyEmailSql, checkOnlyEmailSqlParams);
                if (coeds.Tables[0].Rows.Count != 0)
                {
                    return -1;
                }
            }

            string updateSql = "SP_Update_PersonalInfo";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@CUSTOMER_NAME",customer.CUSTOMER_NAME),
                new SqlParameter("@CNAME",customer.CNAME),
                new SqlParameter("@EMAIL",customer.EMAIL),
                new SqlParameter("@PHONE",customer.PHONE),
                new SqlParameter("@FIXED_TELEPHONE",customer.FIXED_TELEPHONE),
                new SqlParameter("@COMPANY",customer.COMPANY),
                new SqlParameter("@COMPANY_ADDRESS",customer.COMPANY_ADDRESS),
                new SqlParameter("@CUSTOMER_ID",customer.CUSTOMER_ID)
            };
            DataSet ds = DBHelper.GetDataSet(updateSql, sqlParameters, CommandType.StoredProcedure);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Write Login Log
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// Add by bill 2018.8.21
        public void WriteLoginLog(string username,string access_ip)
        {           
            string Proc = "SP_WriteLoginLog";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                    new SqlParameter("@USERNAME",username),
                    new SqlParameter("@ACCESS_IP",access_ip)
            };
            DBHelper.GetDataSet(Proc, sqlParams, CommandType.StoredProcedure);           
        }

    }

}
