using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Utility
{
    public class SendEmailHelper
    {
        private ReadCSVHelper _ReadMultilCSV;
        protected ReadCSVHelper ReadCSV
        {
            get { return _ReadMultilCSV ?? (_ReadMultilCSV = new ReadCSVHelper()); }
        }

        private ReadTXTHelper _ReadEmailTemplate;
        protected ReadTXTHelper ReadTXT
        {
            get { return _ReadEmailTemplate ?? (_ReadEmailTemplate = new ReadTXTHelper()); }
        }

        public string getCode()
        {
            Random rd = new Random();
            return rd.Next(100000, 999999).ToString();

        }
        public void sendResetPasswordEmial(string customer_id, string strClientEmail, string verificationcode, string category)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadResetPasswordEmailMultilingualPath"].ToString().Trim(), category));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(strClientEmail);
       
            ClientEmailMsg.Subject = ds.Tables[0].Rows[0]["Describe"].ToString();
            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["ResetPasswordEmailTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), customer_id, ds.Tables[0].Rows[3]["Describe"].ToString(), verificationcode, ds.Tables[0].Rows[5][2].ToString(),   ds.Tables[0].Rows[4]["Describe"].ToString(),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[6]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[9]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;
            SendEmail(ClientEmailMsg);          
        }
        //Add  by Haskin 2018.08.06  
        public void sendEmail1(string customer_id, string customer_name, string strClientEmail, string activecode, string category)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadEmailMessageMultilingualPath"].ToString().Trim(), category));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(strClientEmail);
            ClientEmailMsg.Subject = ds.Tables[0].Rows[0]["Describe"].ToString();           
            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            string active_link = ds.Tables[0].Rows[10]["Describe"].ToString() + ConfigurationManager.AppSettings["WebSiteAPIPost"].ToString().Trim() + ds.Tables[0].Rows[11]["Describe"].ToString() + customer_id + "&activecode=" + activecode;
            string template;
            if (category == "2")
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadRegisterEmailTemp_en"].ToString());
            }
            else
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadRegisterEmailTemp_ch"].ToString());
            }
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[14]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[3]["Describe"].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), active_link, ds.Tables[0].Rows[12]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[13]["Describe"].ToString(), customer_id, ds.Tables[0].Rows[15]["Describe"].ToString(), ds.Tables[0].Rows[16]["Describe"].ToString());


            //Modify End
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;          
            SendEmail(ClientEmailMsg);
        }
        //End

        public void SendFinishSubscribeEmail(DataSet ds, string Multingual_Id)
        {
            string email = ds.Tables[1].Rows[0]["EMAIL"].ToString();
            string customer_id = ds.Tables[1].Rows[0]["CUSTOMER_ID"].ToString();
            DataSet csv = new DataSet();
            csv.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadFinishSubscribeEmailMultilingualPath"].ToString().Trim(), Multingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(email);
            ClientEmailMsg.Subject = csv.Tables[0].Rows[15]["Describe"].ToString();

            //Modify / Add By Chester 2018.09.03
            //Attachment attachment = new Attachment(ConfigurationManager.AppSettings["FinishSubscribeEmailBackground"].ToString());
            //ClientEmailMsg.Attachments.Add(attachment);

            string[] features = ds.Tables[0].Rows[0]["FEATURES"].ToString().Split('.');
            StringBuilder features_html = new StringBuilder();
            for (int strIndex = 0; strIndex < features.Length - 1; strIndex++)
            {
                features_html.Append("<p style=\"margin: 8px 0; \">");
                features_html.Append(features[strIndex]);
                features_html.Append("</p>");
            }
            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            string template;
            if (Multingual_Id == "2")
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadFinishSubscribeEmailTemp_en"].ToString());
            }
            else
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadFinishSubscribeEmailTemp_ch"].ToString());
            }

            string strMsg = string.Format(template, email_bg, csv.Tables[0].Rows[0]["Describe"].ToString(), csv.Tables[0].Rows[1]["Describe"].ToString(), csv.Tables[0].Rows[2]["Describe"].ToString(), csv.Tables[0].Rows[3]["Describe"].ToString(), csv.Tables[0].Rows[4]["Describe"].ToString(), csv.Tables[0].Rows[5]["Describe"].ToString(), csv.Tables[0].Rows[6]["Describe"].ToString(), csv.Tables[0].Rows[7]["Describe"].ToString(), csv.Tables[0].Rows[8]["Describe"].ToString(), csv.Tables[0].Rows[9]["Describe"].ToString(), csv.Tables[0].Rows[10]["Describe"].ToString(), csv.Tables[0].Rows[11]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), customer_id, ds.Tables[0].Rows[0]["NAMED_USER_COUNT"].ToString(), features_html.ToString(), ds.Tables[0].Rows[0]["ANTI_DDOS"].ToString(), ds.Tables[0].Rows[0]["EMAIL_NOTIFICATION"].ToString(), ds.Tables[0].Rows[0]["SERVER_MONITORING"].ToString(), ds.Tables[0].Rows[0]["SUPPORT_TICKET"].ToString(), csv.Tables[0].Rows[12]["Describe"].ToString(), ds.Tables[0].Rows[0]["VERSION"].ToString(), csv.Tables[0].Rows[16]["Describe"].ToString(), csv.Tables[0].Rows[14]["Describe"].ToString());


            //Modify End

            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;        
            SendEmail(ClientEmailMsg);
        }

        // Modify / Add By Chester 2018.09.06
        public void SendExpireProductEmail(string target_email, string customer_id, string domain_name, string expire_date, string remaining_days, string remaining_hours, string Multingual_Id)
        {
            DataSet csv = new DataSet();
            csv.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadExpireProductEmailMultilingualPath"].ToString().Trim(), Multingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);
            ClientEmailMsg.Subject = csv.Tables[0].Rows[14]["Describe"].ToString();

            string renew_link = ConfigurationManager.AppSettings["WebSiteUrl"].ToString().Trim() + csv.Tables[0].Rows[12]["Describe"].ToString();

            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            string template;
            if (Multingual_Id == "2")
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadExpireProductEmailTem_en"].ToString());
            }
            else
            {
                template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadExpireProductEmailTem_ch"].ToString());
            }

            string strMsg = string.Format(template, email_bg, csv.Tables[0].Rows[0]["Describe"].ToString(), csv.Tables[0].Rows[1]["Describe"].ToString(), csv.Tables[0].Rows[2]["Describe"].ToString(), csv.Tables[0].Rows[3]["Describe"].ToString(), csv.Tables[0].Rows[4]["Describe"].ToString(), csv.Tables[0].Rows[5]["Describe"].ToString(), csv.Tables[0].Rows[6]["Describe"].ToString(), csv.Tables[0].Rows[8]["Describe"].ToString(), csv.Tables[0].Rows[9]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), csv.Tables[0].Rows[10]["Describe"].ToString(), customer_id, domain_name, expire_date, "", "", csv.Tables[0].Rows[7]["Describe"].ToString(), csv.Tables[0].Rows[11]["Describe"].ToString(),renew_link, csv.Tables[0].Rows[13]["Describe"].ToString());           

            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;
            SendEmail(ClientEmailMsg);
        }

        public void SendModifyPasswordVerifyCodeEmail(string target_email, string customer_id, string verifyCode, string Multingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadResetPasswordEmailMultilingualPath"].ToString().Trim(), Multingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);

            ClientEmailMsg.Subject = ds.Tables[0].Rows[0]["Describe"].ToString();
            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadModifyPasswordVerifyCodeEmailTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), customer_id, ds.Tables[0].Rows[3]["Describe"].ToString(), verifyCode, ds.Tables[0].Rows[5][2].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[6]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[9]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        public void SendEmailAfterCancelingAutoPaySuccess(string target_email,string user,string Multilingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadCancelAutoPaySuccessMultilingualPath"].ToString().Trim(), Multilingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);

            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            ClientEmailMsg.Subject = ds.Tables[0].Rows[6]["Describe"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadCancelAutoPayTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(),user, ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[3]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        public void SendEmailAfterCancelingAutoPayFailure(string target_email, string user, string Multilingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadCancelAutoPayFailureMultilingualPath"].ToString().Trim(), Multilingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);

            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            ClientEmailMsg.Subject = ds.Tables[0].Rows[6]["Describe"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadCancelAutoPayTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), user, ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[3]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        public void SendEmailAfterModifyingBankCardSuccess(string target_email,string user,string oldCard,string newCard,string Multilingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadModifyBankCardSuccessEmailMultilingualPath"].ToString().Trim(), Multilingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);

            string prefix = "************";
            oldCard = prefix + oldCard.Substring(oldCard.Length - 4);
            newCard = prefix + newCard.Substring(newCard.Length - 4);

            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            ClientEmailMsg.Subject = ds.Tables[0].Rows[6]["Describe"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadModifyBankCardTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), user, ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[10]["Describe"].ToString(),ds.Tables[0].Rows[9]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[3]["Describe"].ToString(),oldCard,newCard);
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        public void SendEmailToContactCustomer(string target_email,string user,string Multilingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadContactCustomerEmailMultilingualPath"].ToString().Trim(), Multilingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            ClientEmailMsg.To.Add(target_email);

            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            ClientEmailMsg.Subject = ds.Tables[0].Rows[6]["Describe"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadContactCustomerTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), user, ds.Tables[0].Rows[8]["Describe"].ToString(), ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), ds.Tables[0].Rows[4]["Describe"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[3]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        public void SendEmailToContactSellers(string target_email,string customer,DataSet customer_Info,string Multilingual_Id)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadContactSellerEmailMultilingualPath"].ToString().Trim(), Multilingual_Id));
            MailMessage ClientEmailMsg = new MailMessage();
            ClientEmailMsg.From = new MailAddress(ConfigurationManager.AppSettings["SendEmailMailAddress"].ToString().Trim());
            string address=target_email.Replace(';', ',');
            ClientEmailMsg.To.Add(address);

            string sales = ds.Tables[0].Rows[16]["Describe"].ToString();
            string email_bg = ConfigurationManager.AppSettings["LoadEmail_bg"].ToString();
            ClientEmailMsg.Subject = ds.Tables[0].Rows[15]["Describe"].ToString();
            string template = ReadTXT.OpenTXT(ConfigurationManager.AppSettings["LoadContactSellerTemp"].ToString());
            string strMsg = string.Format(template, email_bg, ds.Tables[0].Rows[0]["Describe"].ToString(), ds.Tables[0].Rows[1]["Describe"].ToString(), sales, ds.Tables[0].Rows[2]["Describe"].ToString(), ds.Tables[0].Rows[3]["Describe"].ToString(),customer,ds.Tables[0].Rows[4]["Describe"].ToString(), ds.Tables[0].Rows[5]["Describe"].ToString(), ds.Tables[0].Rows[6]["Describe"].ToString(), customer_Info.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString(), ds.Tables[0].Rows[7]["Describe"].ToString(),customer_Info.Tables[0].Rows[0]["CNAME"].ToString(), ds.Tables[0].Rows[8]["Describe"].ToString(), customer_Info.Tables[0].Rows[0]["EMAIL"].ToString(),ds.Tables[0].Rows[9]["Describe"].ToString(), customer_Info.Tables[0].Rows[0]["COMPANY"].ToString(), ds.Tables[0].Rows[10]["Describe"].ToString(), customer_Info.Tables[0].Rows[0]["PHONE"].ToString(), ds.Tables[0].Rows[11]["Describe"].ToString(), ds.Tables[0].Rows[13]["Describe"].ToString(), ds.Tables[0].Rows[14]["Describe"].ToString(),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ds.Tables[0].Rows[12]["Describe"].ToString());
            ClientEmailMsg.Body = strMsg;
            ClientEmailMsg.IsBodyHtml = true;

            SendEmail(ClientEmailMsg);
        }

        private void SendEmail(MailMessage clientEmailMsg)
        {
            //SmtpClient client = new SmtpClient();
            //client.Host = ConfigurationManager.AppSettings["SendEmailHost"].ToString().Trim();
            //client.Port = int.Parse(ConfigurationManager.AppSettings["SendEmailPort"].ToString().Trim());
            //NetworkCredential credetial = new NetworkCredential();
            //credetial.UserName = ConfigurationManager.AppSettings["SendEmailUsername"].ToString().Trim();
            //credetial.Password = ConfigurationManager.AppSettings["SednEmailPassword"].ToString().Trim();
            //client.Credentials = credetial;
            //client.Send(clientEmailMsg);

            try
            {
                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SendEmailHost"].ToString().Trim(), int.Parse(ConfigurationManager.AppSettings["SendEmailPort"].ToString().Trim()));
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SendEmailUsername"].ToString().Trim(), ConfigurationManager.AppSettings["SednEmailPassword"].ToString().Trim());
                smtpClient.Credentials = credential;
                smtpClient.Send(clientEmailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }        
        //End
    }
}
