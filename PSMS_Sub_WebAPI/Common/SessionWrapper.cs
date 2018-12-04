using PSMS_Sub_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PSMS_Sub_WebAPI.Common
{
    public class SessionWrapper : IRequiresSessionState
    {
        private const string SESSION_CURRENT_USER = "AppSession_CURRENT_USER";

        private const string SESSION_RESETPASSWORD_USER = "AppSession_RESET_USER";   // Add by haskin 2018-9-14

        private const string SESSION_MODIFYPASSWORD_VERIFYCODE = "AppSession_MODIFY_VERIFYCODE";  // Modify / Add by Chester 2018-9-18

        private const string SESSION_MODIFYPERSONALINFO_VERIFYCODE = "AppSession_MODIFYPERSONALINFO_VERIFYCODE"; //Modify / Add by Chester 2018.09.18

        private const string SESSION_CANCELAUTOPAY_VERIFYCODE = "AppSession_CANCELAUTOPAY_VERIFYCODE"; //Add by Chester 2018-10-26

        /// <summary>
        /// CURRENT LOGIN USER INFORMATION<para />
        /// </summary> 
        public static SessionVM CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_CURRENT_USER] != null)
                {
                    return HttpContext.Current.Session[SESSION_CURRENT_USER] as SessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESSION_CURRENT_USER] = value;
            }
        }

        // Add by haskin 2018-9-14
        public static ForgetPasswordSessionVM ResetUser
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_RESETPASSWORD_USER] != null)
                {
                    return HttpContext.Current.Session[SESSION_RESETPASSWORD_USER] as ForgetPasswordSessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESSION_RESETPASSWORD_USER] = value;
            }
        }
        //End

        // Modify / Add By Chester 2018.09.18

        public static EmailVerifyCodeSessionVM ModifyPasswordUser
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_MODIFYPASSWORD_VERIFYCODE] != null)
                {
                    return HttpContext.Current.Session[SESSION_MODIFYPASSWORD_VERIFYCODE] as EmailVerifyCodeSessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESSION_MODIFYPASSWORD_VERIFYCODE] = value;
            }
        }

        public static EmailVerifyCodeSessionVM ModifyPersonalInfoVerifyCode
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_MODIFYPERSONALINFO_VERIFYCODE] != null)
                {
                    return HttpContext.Current.Session[SESSION_MODIFYPERSONALINFO_VERIFYCODE] as EmailVerifyCodeSessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESSION_MODIFYPERSONALINFO_VERIFYCODE] = value;
            }
        }

        public static EmailVerifyCodeSessionVM CancelAutoPayVerifyCode
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_CANCELAUTOPAY_VERIFYCODE] != null)
                {
                    return HttpContext.Current.Session[SESSION_CANCELAUTOPAY_VERIFYCODE] as EmailVerifyCodeSessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESSION_CANCELAUTOPAY_VERIFYCODE] = value;
            }
        }

        // Modify End

        public static void ClearAll()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
        }

    }
}