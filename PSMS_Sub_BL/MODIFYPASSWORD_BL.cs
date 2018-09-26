using PSMS_Sub_DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class MODIFYPASSWORD_BL
    {
        private MODIFYPASSWORD_DA _MODIFYPASSWORD_DA;
        protected MODIFYPASSWORD_DA DA
        {
            get
            {
                return _MODIFYPASSWORD_DA ?? (_MODIFYPASSWORD_DA = new MODIFYPASSWORD_DA());
            }
        }

        /// <summary>
        /// Update the user's password.
        /// </summary>
        /// <param name="username">customer_id</param>
        /// <param name="oldPwd">user's old password</param>
        /// <param name="newPwd">user's new password</param>
        /// <returns>
        /// 0: Incorrect Old Password;
        /// 1:Modify Password Success;
        /// -1:Modify Password Failer;
        /// -2:The old Password Equal New Password;
        /// </returns>
        public int ModifyPassword(string username, string oldPwd, string newPwd)
        {
            string userPwd = DA.SelectOldPassword(username);
            if (userPwd != oldPwd)
            {
                return 0;
            }
            if (userPwd == newPwd)
            {
                return -2;
            }
            if (DA.UpdatePassword(username, newPwd))
            {
                return 1;
            }
            return -1;
        }

    }
}
