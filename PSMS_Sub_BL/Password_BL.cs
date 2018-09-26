using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class PASSWORD_BL
    {
        /// <summary>
        /// Password encryption
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string MD5Encrypt64(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] pwd = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(pwd);
        }
    }
}
