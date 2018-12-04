using PSMS_Sub_DA;
using PSMS_Sub_DM;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
   

    public class PAYMENT_BL
    {
        private PAYMENT_DA _PAYMENT_DA;
        protected PAYMENT_DA DA
        {
            get { return _PAYMENT_DA ?? (_PAYMENT_DA = new PAYMENT_DA()); }
        }

        public int vmQueryByAddPaymentRecord(PAYMENTRECORD data,string Multingual_ID)
        {
            return DA.AddPaymentRecoerd(data,Multingual_ID);
        }

        public DataSet vmQueryBySeletProductPostForm(string category)
        {
            return DA.SeletProductPostForm(category);
        }
        
        public int vmQueryByCreateOrder(PAYMENT payment)
        {
            return DA.CreateOrder(payment);
        }

        public string vmQueryByCheckDomainName(string domain_name)
        {
            return DA.CheckDomainName(domain_name);
        }

        public ResultVM vmQueryByUpdateStatusCode(PaymentStatusCode data)
        {
            return DA.UpdateStatusCode(data);
        }
        
        public ResultVM vmQueryByUpdateOrderExpireDate(OrderExpireDateVM data)
        {
            return DA.UpdateOrderExpireDate(data);
        }

        public int vmQueryByCheckIsCanRenew(string orederref)
        {
            return DA.CheckIsCanRenew(orederref);
        }

        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="XmlFiledValues"></param>
        /// <returns></returns>
        public string vmSaveQueryPaymentStatus(string[] XmlFiledValues)
        {
           return DA.SaveQueryPaymentStatus(XmlFiledValues);
        }

        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="SchXmlFiledValues"></param>
        /// <returns></returns>
        public void vmSaveQuerySchPaymentInfo(string[] SchXmlFiledValues)
        {
    DA.SaveQuerySchPaymentInfo(SchXmlFiledValues);
        }

        /// <summary>
        /// Add by bill 2018-10-19
        /// </summary>
        /// <param name="DetailSchXmlFiledValues"></param>
        /// <param name="mSchPayId"></param>
        /// <returns></returns>
        public void vmSaveQueryDetailSchPaymentInfo(string[] DetailSchXmlFiledValues,string mSchPayId)
        {
            DA.SaveQueryDetailSchPaymentInfo(DetailSchXmlFiledValues, mSchPayId);
        }

        public DataSet vmQueryOrderPayRef(string orderRef)
        {
            return DA.QueryOrderPayRef(orderRef);
        }

        // Add by chester 2018-10-22

        public DataSet vmQueryAutoPayMessage(string merref)
        {
            return DA.QueryAutoPayMessage(merref);
        }

        public DataSet vmQueryDetailSchPay(string schPayId)
        {
            return DA.QueryDetailSchPay(schPayId);
        }

        public DataTable vmGetPaymentMethod()
        {
            return DA.GetPaymentMethod();
        }
        // End

        public void vmSaveChangeCardInfoFeedBack(string[] feedback)
        {
            DA.SaveChangeCardInfoFeedBack(feedback);
        }

        public void vmUpdateCardInfoFeedBack(string orderRef,string mSchPayId)
        {
            DA.UpdateCardInfoFeedBack(orderRef, mSchPayId);
        }

        public string  vmQuerySchPaymentmSchPayId(string orderRef)
        {
            return DA.GetSchPaymentmSchPayId(orderRef);
        }


        public void vmUpdateSchpaymentStatus(string mSchPayId,string orderRef)
        {
            DA.UpdateSchPaymentStatus(mSchPayId, orderRef);
        }

        // Add by Chester 2018.10.31
        public string vmQuerySchPaymentAccount(string orderRef)
        {
            return DA.GetSchPaymentAccount(orderRef);
        }
        // End
    }
}
