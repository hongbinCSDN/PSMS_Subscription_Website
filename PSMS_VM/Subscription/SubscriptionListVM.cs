using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_VM.Subscription
{
    public class SubscriptionListVM
    {
        public string RECORD_ID { get; set; }
        public string ORDERREF { get; set; }
        public string DOMAIN_NAME { get; set; }
        public string CREATE_TIME { get; set; }
        public string PRODUCT_CR_TIME { get; set; }
        public string PRODUCT_AT_TIME { get; set; }
        public string STATUS { get; set; }
        public string PAYMENT_TYPE_ID { get; set; }  //Add by bill 2018.9.3
    }
}
