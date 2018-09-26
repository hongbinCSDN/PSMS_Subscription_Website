using PSMS_Sub_BL;
using PSMS_Sub_WebAPI.Common;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.Payment
{
    [WebApiTracker,Authorize, Authenticate]
    public class ExternalInterfaceController : ApiController
    {

        private PAYMENT_BL _PAYMENT_BL;
        protected PAYMENT_BL BL_PAYMENT
        {
            get { return _PAYMENT_BL ?? (_PAYMENT_BL = new PAYMENT_BL()); }
        }

        [Route("Payment/ToStatusCode")]      
        [HttpPost]
        public async Task<IHttpActionResult> pUpdateStatusCode(PaymentStatusCode data) 
        {
            return await Task.FromResult(UpdateStatusCode(data));
        } 

        public IHttpActionResult UpdateStatusCode(PaymentStatusCode data)
        {
            try
            {
                return Ok(BL_PAYMENT.vmQueryByUpdateStatusCode(data));
            }
            catch(Exception e)
            {
                return Ok(e);
            }
        }

        [Route("Payment/UpdateOrderExpireDate")]
        [HttpPost]
        public async Task<IHttpActionResult> pUpdateOrderExpireDate(OrderExpireDateVM data)
        {
            return await Task.FromResult(UpdateOrderExpireDate(data));
        }

        public IHttpActionResult UpdateOrderExpireDate(OrderExpireDateVM data)
        {
            try
            {
                return Ok(BL_PAYMENT.vmQueryByUpdateOrderExpireDate(data));
            }
            catch(Exception e)
            {
                return Ok(e);
            }
        }


    }
}
