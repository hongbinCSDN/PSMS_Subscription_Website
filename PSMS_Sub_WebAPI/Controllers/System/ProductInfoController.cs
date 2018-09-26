using PSMS_Sub_BL;
using PSMS_Sub_WebAPI.Common;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    [WebApiTracker, Authenticate]
    public class ProductInfoController : ApiController
    {
        private PRODUCTINFO_BL _PRODUCTINFO_BL;
        protected PRODUCTINFO_BL BL_PRODUCTINFO
        {
            get
            {
                return _PRODUCTINFO_BL ?? (_PRODUCTINFO_BL = new PRODUCTINFO_BL());
            }
        }

            
        ///Add by bill 2018.8.7 
        /// <summary>
        /// Get product info by username
        /// </summary>
        /// <returns></returns>
        [Route("System/GetProductByUser")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetProductInfomation()
        {
            return await Task.FromResult(GetProductInfomation());
        }

        public IHttpActionResult GetProductInfomation()
        {
            try
            {
                return Ok(BL_PRODUCTINFO.vmQueryProuctInfoByUsername(SessionWrapper.CurrentUser.CustomerID, CacheWrapper.CurrentMultilingual.MultilingualID));
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        // Modify / Add By Chester 2018.08.08
        //[Route("System/CheckStatusByUser")]
        //[HttpGet]
        //[Authorize]
        //public async Task<IHttpActionResult> pCheckStatusByUsername()
        //{
        //    return await Task.FromResult(CheckStatusByUsername());
        //}

        //public IHttpActionResult CheckStatusByUsername()
        //{
        //    try
        //    {
        //        string status = BL_PRODUCTINFO.vmQueryCheckStatusByUsername(SessionWrapper.CurrentUser.CustomerID);
        //        return Ok(status);
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e);
        //    }
        //}
        //Modify End

        //End



        /// <summary>
        /// Get The Product Information After Payment
        /// </summary>
        /// <returns></returns>

        [Route("System/GetProductInfo")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetProductInfo(string orderref)
        {
            return await Task.FromResult(GetProductInfo(orderref));
        }

        public IHttpActionResult GetProductInfo(string orderref)
        {
            try
            {
                return Ok(BL_PRODUCTINFO.vmQueryByProductInfo(SessionWrapper.CurrentUser.CustomerID, orderref,CacheWrapper.CurrentMultilingual.MultilingualID));
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        /// <summary>
        /// Get The Product'Status Percent After Payment
        /// </summary>
        /// <returns></returns>
        [Route("System/GetStatusPercent")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetStatusPercent(string orderref)
        {
            return await Task.FromResult(GetStatusPercent(orderref));
        }

        public IHttpActionResult GetStatusPercent(string orderref)
        {
            try
            {
                string percentage = BL_PRODUCTINFO.vmQueryByStatusPercent(SessionWrapper.CurrentUser.CustomerID, orderref);
                return Ok(percentage);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/GetStatusPercentByUser")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pGetStatusPercentByUsername()
        {
            return await Task.FromResult(GetStatusPercentByUsername());
        }

        public IHttpActionResult GetStatusPercentByUsername()
        {
            try
            {
                string percentage = BL_PRODUCTINFO.vmQueryGetStatusPercentByUsername(SessionWrapper.CurrentUser.CustomerID);
                return Ok(percentage);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

    }
}
