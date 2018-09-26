using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_DA
{
    public class LANGUAGE_DA
    {
        public DataTable GetLanguageCategory()
        {
            string sSql = "SELECT * FROM SYS_T_DICTIONARY WHERE CATEGORY='LUNGUAGE' AND STATUES=1";
            DataSet ds = DBHelper.GetDataSet(sSql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0].Copy();
            return dt;
        }
    }
}
