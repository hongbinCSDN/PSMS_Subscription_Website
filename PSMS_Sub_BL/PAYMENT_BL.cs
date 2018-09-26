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

        public string vmQueryByUpdateStatusCode(PaymentStatusCode data)
        {
            return DA.UpdateStatusCode(data);
        }
        
        public int vmQueryByUpdateOrderExpireDate(OrderExpireDateVM data)
        {
            return DA.UpdateOrderExpireDate(data);
        }

        public int vmQueryByCheckIsCanRenew(string orederref)
        {
            return DA.CheckIsCanRenew(orederref);
        }
    }
}
