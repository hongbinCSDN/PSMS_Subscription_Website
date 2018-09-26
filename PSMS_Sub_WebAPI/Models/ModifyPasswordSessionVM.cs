using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSMS_Sub_WebAPI.Models
{
    public class ModifyPasswordSessionVM
    {
        private string emailVerifyCode;
        public string EmailVerifyCode
        {
            get
            {
                return emailVerifyCode;
            }
            set
            {
                emailVerifyCode = value;
            }
        }
        private DateTime expiredTime;
        public DateTime ExpiredTime
        {
            get
            {
                return expiredTime;
            }
            set
            {
                expiredTime = value;
            }
        }
    }
}