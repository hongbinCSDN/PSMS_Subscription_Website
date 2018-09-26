using PSMS_Sub_DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class EXPIREPRODUCTEMAIL_BL
    {
        private EXPIREPRODUCTEMAIL_DA _EXPIREPRODUCTEMAIL;

        protected EXPIREPRODUCTEMAIL_DA DA_EXPIREPRODUCTEMAIL
        {
            get { return _EXPIREPRODUCTEMAIL ?? (_EXPIREPRODUCTEMAIL = new EXPIREPRODUCTEMAIL_DA()); }
        }

        public int QuerySendEmailForExpireProduct()
        {
            return DA_EXPIREPRODUCTEMAIL.SendEmailForExpireProduct();
        }
    }
}

