using PSMS_Utility;
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
    public class EXPIREPRODUCTEMAIL_DA
    {

        private SendEmailHelper _SendEmailHelper;

        protected SendEmailHelper sendemail
        {
            get { return _SendEmailHelper ?? (_SendEmailHelper = new SendEmailHelper()); }
        }
        public int SendEmailForExpireProduct()
        {
            string Proc = "SP_Notification_SelectExpireProductUser";
            DataSet ds = DBHelper.GetDataSet(Proc, null, CommandType.StoredProcedure);
            string ExpireSendDays = ConfigurationManager.AppSettings["ExpireSendDays"].ToString().Trim();
            string[] ExpireDaysArray = ExpireSendDays.Split('/');
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TimeSpan ts = Convert.ToDateTime(dr["PRODUCT_AT_TIME"].ToString()) - System.DateTime.Now;
                    int day = ts.Days;
                    int hours = ts.Hours;
                    for(int i = 0; i < ExpireDaysArray.Length; i++)
                    {
                        if(day.ToString() == ExpireDaysArray[i])
                        {
                            //string ExpireDate = Convert.ToDateTime(dr["PRODUCT_AT_TIME"].ToString()).ToShortDateString().ToString();
                            string ExpireDate = Convert.ToDateTime(dr["PRODUCT_AT_TIME"].ToString()).ToString("yyyy-MM-dd");
                            string Customer_ID = dr["CUSTOMER_ID"].ToString();
                            string Email = dr["EMAIL"].ToString();
                            string Domain_Name = dr["DOMAIN_NAME"].ToString() + ConfigurationManager.AppSettings["WebsitePostfix"].ToString();
                            string Multiling_Id = dr["MULTILINGUAL_ID"].ToString();
                            sendemail.SendExpireProductEmail(Email, Customer_ID, Domain_Name, ExpireDate, day.ToString(), hours.ToString(), Multiling_Id);
                        }
                    }
                    //if (day == 7 || day == 3 || day == 1)
                    //{
                    //    string ExpireDate = Convert.ToDateTime(dr["PRODUCT_AT_TIME"].ToString()).ToShortDateString().ToString();
                    //    string Customer_ID = dr["CUSTOMER_ID"].ToString();
                    //    string Email = dr["EMAIL"].ToString();
                    //    string Domain_Name = dr["DOMAIN_NAME"].ToString() + ConfigurationManager.AppSettings["WebsitePostfix"].ToString();
                    //    string Multiling_Id = dr["MULTILINGUAL_ID"].ToString();
                    //    sendemail.SendExpireProductEmail(Email, Customer_ID, Domain_Name, ExpireDate, day.ToString(), hours.ToString(), Multiling_Id);
                    //}
                }
            }
            return 1;
        }
    }
}
