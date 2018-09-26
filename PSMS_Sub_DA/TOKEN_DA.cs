using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DA
{
    public class TOKEN_DA
    {
        public string CheckToken(string sToken,string username)
        {
            //bool bToken = false;
            string sSql = "SP_CheckTokenValues";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@Token",sToken),  //strToHexByte(sToken)
                new SqlParameter("@USERNAME",username)
            };

            DataSet ds = DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
            return ds.Tables[0].Rows[0]["Result"].ToString();
            //if (ds.Tables[0].Rows[0]["Result"].ToString() == "1")
            //{
            //    bToken = true;
            //}
            //return bToken;
        }

        public void SaveToken(CustomerVM session)
        {
            string sSql = "SP_UpdateUserToken";
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@TOKEN",session.TOKEN),
                new SqlParameter("@USERNAME",session.CUSTOMERID)
            };
            DBHelper.GetDataSet(sSql, sqlParams, CommandType.StoredProcedure);
        }

        /// <summary>
        /// The string goes to a hexadecimal byte array
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
