using PSMS_Sub_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PSMS_Sub_WebAPI.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Authenticate : AuthorizationFilterAttribute
    {
        private TOKEN_BL _TOKEN_BL;
        protected TOKEN_BL BL_TOKEN
        {
            get { return _TOKEN_BL ?? (_TOKEN_BL = new TOKEN_BL());}
        }

        private GetSystmPromptMessage _GetSystmPromptMessage;
        protected GetSystmPromptMessage GetSystmPrompt_Message
        {
            get { return _GetSystmPromptMessage ?? (_GetSystmPromptMessage = new GetSystmPromptMessage()); }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                //Get Token
                string sToken = actionContext.Request.Headers.Authorization.Parameter;               
                if (sToken == null || BL_TOKEN.CheckToken(sToken, SessionWrapper.CurrentUser.CustomerID) != "1") //!BL_TOKEN.CheckToken(sToken, SessionWrapper.CurrentUser.CustomerID)
                {
                    if(BL_TOKEN.CheckToken(sToken, SessionWrapper.CurrentUser.CustomerID) == "-1")
                    {
                        HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, GetSystmPrompt_Message.GetMessage("33"));  //"您没有登录，不能访问此模块！"                  
                        actionContext.Response = response;
                    }
                    else
                    {
                        HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, GetSystmPrompt_Message.GetMessage("21"));  //"您没有登录，不能访问此模块！"                  
                        actionContext.Response = response;
                        return;
                    }
                    
                }

            }
            catch(Exception e)
            {
                HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
                actionContext.Response = response;
                return;
            }
        }
    }
}