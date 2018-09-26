using PSMS_Sub_BL;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    public class MultilingualController : ApiController
    {
        private ReadCSVHelper _ReadMultilCSV;
        protected ReadCSVHelper ReadCSV
        {
            get { return _ReadMultilCSV ?? (_ReadMultilCSV = new ReadCSVHelper()); }
        }

        private LANGUAGE_BL _LANGUAGE_BL;

        protected LANGUAGE_BL BL_Language
        {
            get { return _LANGUAGE_BL ?? (_LANGUAGE_BL = new LANGUAGE_BL()); }
        }


        /// <summary>
        /// Multilingual Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IHttpActionResult SaveMultilingualSession(string category)
        {
            MultilingualSessionVM vm = new MultilingualSessionVM() { MultilingualID = category };
            CacheWrapper.CurrentMultilingual = vm;
            return Ok();
        }

        /// <summary>
        ///  page Multilingual change
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("Multilingual/{pagename}/{category}")]
        [HttpGet]
        public async Task<IHttpActionResult> pMultilingualAsync(string pagename,string category)
        {
            return await Task.FromResult(Multilingual(pagename,category));
        }

        /// <summary>
        ///  page Multilingual change
        /// </summary>
        /// <returns></returns>       
        public IHttpActionResult Multilingual(string pagename, string category)
        {
            try
            {
                SaveMultilingualSession(category);               
                string url = ConfigurationManager.AppSettings["Load" + pagename + "MultilingualPath"].ToString().Trim();
                DataSet ds = new DataSet();
                ds.Tables.Add(ReadCSV.OpenCSV(url, category));
                ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadSystemMessagePath"].ToString().Trim(), category));
                ds.Tables.Add(ReadCSV.OpenCSV(ConfigurationManager.AppSettings["LoadNavigationMultilingualPath"].ToString().Trim(), category));
                ds.Tables.Add(BL_Language.vmGetLanguageCategory());
                return Ok(ds);
               
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

    }
}
