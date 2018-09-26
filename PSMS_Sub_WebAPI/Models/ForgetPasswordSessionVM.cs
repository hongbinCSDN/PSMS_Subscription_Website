using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSMS_Sub_WebAPI.Models
{
    public class ForgetPasswordSessionVM
    {

        private string customerid;
        //Add by Haskin 2018.07.27
        private string resetpasswordemailcode;
        //Add by Haskin 2018.09.06
        private bool isEmailcode = false;

        private DateTime emaileodeExpireDate;

        //Add by Haskin 2018.07.27
        public string Resetpasswordemailcode
        {
            get { return resetpasswordemailcode; }
            set { resetpasswordemailcode = value; }
        }

        public string CustomerID
        {
            get { return customerid; }
            set { customerid = value; }
        }



        public bool IsEmailcode
        {
            get { return isEmailcode; }
            set { isEmailcode = value; }
        }

        /// <summary>
        /// Add by bill 2018-9-19
        /// </summary>
        public DateTime EmailCodeExpireDate
        {
            get { return emaileodeExpireDate; }
            set { emaileodeExpireDate = value; }
        }
    }
}