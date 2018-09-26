using PSMS_Sub_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.Notification
{
    public class ProductExpireController : ApiController
    {

        private EXPIREPRODUCTEMAIL_BL _EXPIREPRODUCTEMAIL;

        protected EXPIREPRODUCTEMAIL_BL BL_EXPIREPRODUCTEMAIL
        {
            get { return _EXPIREPRODUCTEMAIL ?? (_EXPIREPRODUCTEMAIL = new EXPIREPRODUCTEMAIL_BL()); }
        }

        [Route("Notification/SendEmailForExpireProd")]
        [HttpGet]
        public async Task<IHttpActionResult> pSendProdExpireEmail()
        {
            return await Task.FromResult(SendProdExpireEmail());
        }

        public IHttpActionResult SendProdExpireEmail()
        {
            return Ok(BL_EXPIREPRODUCTEMAIL.QuerySendEmailForExpireProduct());
        }
    }
}
