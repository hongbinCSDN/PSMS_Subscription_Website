using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PSMS_Sub_WebAPI.Common
{
    public class GetSystmPromptMessage
    {
        private ReadCSVHelper _ReadCSV;
        protected ReadCSVHelper ToReadCSV
        {
            get { return _ReadCSV ?? (_ReadCSV = new ReadCSVHelper()); }
        }

        private string path = ConfigurationManager.AppSettings["LoadSystemMessagePath"].ToString().Trim();


        public  string GetMessage(string MessageIdentifying)
        {
            
            string MultilingualID = CacheWrapper.CurrentMultilingual.MultilingualID;
            string Message = ToReadCSV.OpenCSVWithIdentifying(path, MultilingualID, MessageIdentifying);
            return Message;
        }
    }
}