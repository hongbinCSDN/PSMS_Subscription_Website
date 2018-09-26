using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PSMS_Sub_DM
{
    public class WebApiMonitorLog
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public DateTime ExecuteStartTime { get; set; }
        public DateTime ExecuteEndTime { get; set; }

        /// <summary>
        /// The action parameter of the request
        /// </summary>
        public Dictionary<string, object> ActionParams { get; set; }
        /// <summary>
        /// Http request header
        /// </summary>
        public string HttpRequestHeaders { get; set; }
        /// <summary>
        /// Request Method
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// Request IP
        /// </summary>
        public string IP { get; set; }
        public string GetLogInfo()
        {
            string Msg = @"
            Action execution time monitoring:
            ContrillerName:{0}Controller
            ActionName:{1}
            StartTime:{2}
            EndTime:{3}
            TotalTime:{4}秒
            Action Params:{5}
            Http Request Headers:{6}
            Client IP:{7},
            HttpMethod:{8}
            ";
            return string.Format(Msg,
                ControllerName,
                ActionName,
                ExecuteStartTime,
                ExecuteEndTime,
                (ExecuteEndTime - ExecuteStartTime).TotalSeconds,
                GetCollections(ActionParams),
                HttpRequestHeaders,
                GetIP(),
                HttpMethod);
        }

        public string GetCollections(Dictionary<string, object> Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;

        }

        public string GetIP()
        {
            //string ip = string.Empty;
            //if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
            //    ip = Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            //if (string.IsNullOrEmpty(ip))
            //    ip = Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            //return ip;
            String clientIP = "";
            if (System.Web.HttpContext.Current != null)
            {
                clientIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIP) || (clientIP.ToLower() == "unknown"))
                {
                    clientIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(clientIP))
                    {
                        clientIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    clientIP = clientIP.Split(',')[0];
                }
            }
            return clientIP;
        }
    }
}
