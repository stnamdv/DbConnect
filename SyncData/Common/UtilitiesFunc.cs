using log4net;
using SyncData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SyncData.Common
{
    public class UtilitiesFunc
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetHostName()
        {
            string http = HttpContext.Current.Request.Url.Scheme;
            string host = HttpContext.Current.Request.Url.Authority;
            string ApplicationAlias = HostingEnvironment.ApplicationVirtualPath;
            if (ApplicationAlias != "/")
                ApplicationAlias = ApplicationAlias + "/";
            return http + "://" + host + ApplicationAlias;
        }

        public string GetIPAddress()
        {
            try
            {
                string IPAddress;
                IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IPAddress))
                    IPAddress = HttpContext.Current.Request.Params["REMOTE_ADDR"];
                return IPAddress;
            }
            catch
            {
                return "::";
            }
        }

        public void WriteLogInfo(string iP, string message)
        {
            try
            {
                Log.InfoFormat("Execute: {0}, IP: {1}", message, iP);
            }
            catch
            {
            }
        }

        public void WriteLogError(string iP, object message)
        {
            try
            {
                Log.FatalFormat("Execute fail: {0}, IP: {1}", message, iP);
            }
            catch
            {
            }
        }

        public void WriteLogError(string iP, Exception message)
        {
            try
            {
                Log.FatalFormat("Execute fail: {0}, IP: {1}", message, iP);
            }
            catch
            {
            }
        }

        public void SetDbSession(DatabaseInfo _obj)
        {
            if (_obj.TYPE == "SOURCE")
            {
                HttpContext.Current.Session["DatabaseInfoSrc"] = _obj;
            }
            else
            {
                HttpContext.Current.Session["DatabaseInfoDes"] = _obj;
            }
        }

        public void ClearDbSession(string type)
        {
            if (type == "1")
            {
                HttpContext.Current.Session["DatabaseInfoSrc"] = null;
            }
            else
            {
                HttpContext.Current.Session["DatabaseInfoDes"] = null;
            }
        }

        public DatabaseInfo GetDbSessionSrc()
        {
            try
            {
                return HttpContext.Current.Session["DatabaseInfoSrc"] as DatabaseInfo;
            }
            catch (Exception ex)
            {
                WriteLogError(GetIPAddress(), ex);
                return null;
            }
        }

        public DatabaseInfo GetDbSessionDes()
        {
            try
            {
                return HttpContext.Current.Session["DatabaseInfoDes"] as DatabaseInfo;
            }
            catch (Exception ex)
            {
                WriteLogError(GetIPAddress(), ex);
                return null;
            }
        }
    }
}