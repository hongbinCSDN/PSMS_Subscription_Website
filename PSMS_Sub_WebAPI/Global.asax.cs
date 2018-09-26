using PSMS_Sub_WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace PSMS_Sub_WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new WebApiTrackerAttribute()); //Add by bill 2018.8.21
        }

        public override void Init()
        {
            this.PostAuthenticateRequest += (Sender, e) => HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            base.Init();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started


        }

        protected void Session_End()
        {
            //if (StaticLibrary.ActiveSession.ContainsKey(Session.SessionID))
            //{
            //    StaticLibrary.ActiveSession.Remove(Session.SessionID);
            //}
        }


    }
}
