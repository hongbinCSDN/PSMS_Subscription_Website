using PSMS_Sub_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSMS_Sub_WebAPI.Common
{
    public class CacheWrapper
    {
        private const string SESSION_CURRENT_MULTILINGUAL = "AppSession_CURRENT_MULTILINGUAL";

        public static MultilingualSessionVM CurrentMultilingual
        {
            get
            {
                if (HttpContext.Current.Cache[SESSION_CURRENT_MULTILINGUAL] != null)
                {
                    return HttpContext.Current.Cache[SESSION_CURRENT_MULTILINGUAL] as MultilingualSessionVM;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Cache[SESSION_CURRENT_MULTILINGUAL] = value;
            }
        }
    }
}