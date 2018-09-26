using Microsoft.Owin.Security.OAuth;
using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PSMS_Sub_WebAPI.Common
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private CUSTOMER_BL _CUSTOMER_BL;
        protected CUSTOMER_BL BL_CUSTOMER
        {
            get { return _CUSTOMER_BL ?? (_CUSTOMER_BL = new CUSTOMER_BL()); }
        }

        private CUSTOMER _CUSTOMER;
        protected CUSTOMER customer
        {
            get { return _CUSTOMER ?? (_CUSTOMER = new CUSTOMER()); }
        }

        private PASSWORD_BL _PASSWORD_BL;
        protected PASSWORD_BL BL_PASSWORD
        {
            get { return _PASSWORD_BL ?? (_PASSWORD_BL = new PASSWORD_BL()); }
        }

        private GetSystmPromptMessage _GetSystmPromptMessage;
        protected GetSystmPromptMessage GetSystmPrompt_Message
        {
            get { return _GetSystmPromptMessage ?? (_GetSystmPromptMessage = new GetSystmPromptMessage()); }
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            /*
             * Data validation for username and password 
             * 
            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }*/

            //customer.CUSTOMER_ID = context.UserName;
            //customer.PASSWORD = BL_PASSWORD.MD5Encrypt64(context.Password);
            //var data =  BL_CUSTOMER.vmQueryByLogin(customer);          
            //if (data.Message == "False")
            //{   
            //    string message = GetSystmPrompt_Message.GetMessage("2");
            //    context.SetError("invalid_grant", message);  
            //    return;
            //}
            //else if(data.Message == "Inactive")
            //{
            //    string message = GetSystmPrompt_Message.GetMessage("8");
            //    context.SetError("invalid_grant", message);
            //    return;
            //}
            //else
            //{              
            //    BL_CUSTOMER.vmQueryByLoginLog(customer.CUSTOMER_ID);  
            //}
          
            context.Validated(identity);
                                          
        }

    }
}