using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DA
{
    public class MODIFYPASSWORD_DA
    {
        /// <summary>
        /// Query the user's old password
        /// </summary>
        /// <param name="username">Customer_id</param>
        /// <returns>User's Old Password</returns>
        public string SelectOldPassword(string username)
        {
            string sSql = "SELECT * FROM [SYS_T_CUSTOMER] WHERE CUSTOMER_ID = @USERNAME";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@USERNAME",username)
            };
            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams);
            return ds.Tables[0].Rows[0]["PASSWORD"].ToString();
        }

        /// <summary>
        /// Update the User's password,if afected count > 0,return true.Otherwise,return false.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPwd"></param>
        /// <returns>True: Update Success;False: Update Failer</returns>
        public bool UpdatePassword(string username,string newPwd)
        {
            string sSql = "SP_Personal_UpdatePassword";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@NEWPASSWORD",newPwd),
                new SqlParameter("@USERNAME",username)
            };
            int count = DBHelper.Exec(sSql, sqlParams, CommandType.StoredProcedure);
            if (count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
