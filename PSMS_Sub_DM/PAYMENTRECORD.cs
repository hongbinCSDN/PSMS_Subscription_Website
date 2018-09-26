using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DM
{
    public class PAYMENTRECORD
    {
        public string RECORDID { get; set; }
        public int PRC { get; set; }
        public int SRC { get; set; }
        public string ORD { get; set; }
        public string REF { get; set; }
        public int PAYREF { get; set; }
        public int SUCCESSCODE { get; set; }
        public string AMT { get; set; }
        public string CUR { get; set; }
        public string HOLDER { get; set; }
        public string AUTHID { get; set; }
        public string ALERTCODE { get; set; }
        public string REMARK { get; set; }
        public string ECI { get; set; }
        public string PAYERAUTH { get; set; }
        public string SOURCEIP { get; set; }
        public string IPCOUNTRY { get; set; }
        public string PAYMETHOD { get; set; }
        public string CARDLSSUINGCOUNTRY { get; set; }
        public string CHANNELTYPE { get; set; }
    }
}
