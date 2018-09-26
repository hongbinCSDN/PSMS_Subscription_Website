using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DM
{
    public class PAYMENT
    {
        public int RECORD_ID { get; set; }
        public string ORDERREF { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string STATUS_CODE { get; set; }
        public string STATUS_PAYMENT { get; set; }
        public decimal AMOUNT { get; set; }
        public string PAYMENT_TYPE_ID { get; set; }
        public string DOMAIN_NAME { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string RENEWAL_LAST_ORDERREF { get; set; }
        public string MULTILINGUAL_ID { get; set; }
    }
}
