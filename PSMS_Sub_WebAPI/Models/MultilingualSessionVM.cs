using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSMS_Sub_WebAPI.Models
{
    public class MultilingualSessionVM
    {
        private string multilingualid;
        
        public string MultilingualID
        {
            get { return multilingualid; }
            set { multilingualid = value; }
        }
    }
}