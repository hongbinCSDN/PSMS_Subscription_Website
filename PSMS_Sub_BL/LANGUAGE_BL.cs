using PSMS_Sub_DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class LANGUAGE_BL
    {
        private LANGUAGE_DA _LANGUAGE_DA;

        /// <summary>
        /// 
        /// </summary>
        protected LANGUAGE_DA DA
        {
            get { return _LANGUAGE_DA ?? (_LANGUAGE_DA = new LANGUAGE_DA()); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable vmGetLanguageCategory()
        {
            return DA.GetLanguageCategory();
        }
    }
}
