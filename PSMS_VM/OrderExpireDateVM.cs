using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_VM
{
    public class OrderExpireDateVM
    {
        public string RECORD_ID { get; set; }
        public string ORDERREF { get; set; }
        public DateTime PRODUCT_CR_TIME { get; set; }
        public DateTime PRODUCT_AT_TIME { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
