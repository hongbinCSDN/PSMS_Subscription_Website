using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_VM.Common
{
    
    public class SessionVM
    {
        private string tokenid;
        private string customerid;

        public string TokenID
        {
            get { return tokenid; }
            set { tokenid = value; }

        }
        public string CustomerID
        {
            get { return customerid; }
            set { customerid = value; }
        }

    }
}
