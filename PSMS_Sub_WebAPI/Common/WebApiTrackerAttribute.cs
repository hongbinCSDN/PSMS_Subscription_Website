﻿using PSMS_Sub_DM;
using PSMS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PSMS_Sub_WebAPI.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class WebApiTrackerAttribute : ActionFilterAttribute
    {
        private readonly string Key = "_thisWebApiOnActionMonitorLog_";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            WebApiMonitorLog MonLog = new WebApiMonitorLog();
            MonLog.ExecuteStartTime = DateTime.Now;
            //获取Action 参数
            MonLog.ActionParams = actionContext.ActionArguments;
            MonLog.HttpRequestHeaders = actionContext.Request.Headers.ToString();
            MonLog.HttpMethod = actionContext.Request.Method.Method;

            actionContext.Request.Properties[Key] = MonLog;
            var form = System.Web.HttpContext.Current.Request.Form;
            #region 如果参数是实体对象，获取序列化后的数据
            Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            //stream.Position = 0;
            string responseData = "";
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                responseData = reader.ReadToEnd().ToString();
            }
            if (!string.IsNullOrWhiteSpace(responseData) && !MonLog.ActionParams.ContainsKey("__EntityParamsList__"))
            {
                MonLog.ActionParams["__EntityParamsList__"] = responseData;
            }
            #endregion
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            WebApiMonitorLog MonLog = actionExecutedContext.Request.Properties[Key] as WebApiMonitorLog;
            MonLog.ExecuteEndTime = DateTime.Now;
            MonLog.ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            MonLog.ControllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            LoggerHelper.Monitor(MonLog.GetLogInfo());
            if (actionExecutedContext.Exception != null)
            {
                string Msg = string.Format(@"
                Request [{0}Controller]'s [{1}] cause abnormal：
                Action Params : {2}
                Http Request Headers : {3}
                Client IP : {4},
                HttpMethod : {5}
                    ", MonLog.ControllerName, MonLog.ActionName, MonLog.GetCollections(MonLog.ActionParams), MonLog.HttpRequestHeaders, MonLog.GetIP(), MonLog.HttpMethod);
                LoggerHelper.Error(Msg, actionExecutedContext.Exception);
            }
        }
    }
}