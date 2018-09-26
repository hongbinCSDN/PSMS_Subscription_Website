using PSMS_Sub_DA;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class TOKEN_BL
    {
        private TOKEN_DA _TOKEN_DA;
        protected TOKEN_DA DA
        {
            get { return _TOKEN_DA ?? (_TOKEN_DA = new TOKEN_DA()); }
        }

        public string CheckToken(string sToken,string username)
        {
            return DA.CheckToken(sToken,username);
        }

        public void SaveTokenToDB(CustomerVM session)
        {
            DA.SaveToken(session);   
        }
    }
}
