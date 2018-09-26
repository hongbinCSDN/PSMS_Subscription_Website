using PSMS_Sub_WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    [Authenticate]
    public class AccessController : ApiController
    {
    }
}
