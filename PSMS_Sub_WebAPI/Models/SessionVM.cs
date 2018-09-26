using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_WebAPI.Models
{
    
    public class SessionVM
    {
        private string tokenid;
        private string customerid;

        //Add by bill 2018-9-17
        private string email;
        private string logintime;
        //End
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
        /// <summary>
        /// Add by bill 2018-9-17
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        /// <summary>
        /// Add by bill 2018-9-17
        /// </summary>
        public string LoginTime
        {
            get { return logintime; }
            set { logintime = value; }
        }
         
       

    }
}
