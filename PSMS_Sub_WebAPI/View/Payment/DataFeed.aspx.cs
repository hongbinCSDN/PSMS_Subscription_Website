using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSMS_Sub_WebAPI.View.Payment
{
    public partial class DataFeed : System.Web.UI.Page
    {
        private PAYMENT_BL _PAYMENT_BL;
        protected PAYMENT_BL BL_PAYMENT
        {
            get { return _PAYMENT_BL ?? (_PAYMENT_BL = new PAYMENT_BL()); }
        }

        private PAYMENTRECORD _PAYMENTRECORD;
        protected PAYMENTRECORD paymentrecord
        {
            get { return _PAYMENTRECORD ?? (_PAYMENTRECORD = new PAYMENTRECORD()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {          
            Response.Write("OK");
            paymentrecord.PRC = int.Parse(Request.Form["prc"].ToString());
            paymentrecord.SRC = int.Parse(Request.Form["src"].ToString());
            paymentrecord.ORD = Request.Form["Ord"].ToString();
            paymentrecord.REF = Request.Form["Ref"].ToString();
            paymentrecord.PAYREF = int.Parse(Request.Form["PayRef"].ToString());
            paymentrecord.SUCCESSCODE = int.Parse(Request.Form["successcode"].ToString());
            paymentrecord.AMT = Request.Form["Amt"].ToString();
            paymentrecord.CUR = Request.Form["Cur"].ToString();
            paymentrecord.HOLDER = Request.Form["Holder"].ToString();
            paymentrecord.AUTHID = Request.Form["AuthId"].ToString();
            paymentrecord.ALERTCODE = Request.Form["AlertCode"].ToString();
            paymentrecord.REMARK = Request.Form["remark"].ToString();
            paymentrecord.ECI = Request.Form["eci"].ToString();
            paymentrecord.PAYERAUTH = Request.Form["payerAuth"].ToString();
            paymentrecord.SOURCEIP = Request.Form["sourceIp"].ToString();
            paymentrecord.IPCOUNTRY = Request.Form["ipCountry"].ToString();
            paymentrecord.PAYMETHOD = Request.Form["payMethod"].ToString();
            paymentrecord.CARDLSSUINGCOUNTRY = Request.Form["cardIssuingCountry"].ToString();
            paymentrecord.CHANNELTYPE = Request.Form["channelType"].ToString();



            if (!Page.IsPostBack)
            {
                if (paymentrecord.SUCCESSCODE == 0)
                {                 
                    BL_PAYMENT.vmQueryByAddPaymentRecord(paymentrecord, CacheWrapper.CurrentMultilingual.MultilingualID); 
                }
            } 
        }
    }
}