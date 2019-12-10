using SyncData.Common;
using SyncData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SyncData.DataAccess
{
    public class DatabaseInfoCtrl
    {
        private UtilitiesFunc utilities = new UtilitiesFunc();
        private string IpAddr;
        private CommonDA objCtr;
        private string errMsg;

        public DatabaseInfoCtrl()
        {
            IpAddr = utilities.GetIPAddress();
        }

        public bool TestConnect(DatabaseInfo _obj, ref string errMsg)
        {
            try
            {
                utilities.WriteLogInfo(IpAddr, "Connected");

                DB db = new DB(_obj.CONNECTIONSTRING);
                try
                {
                    db.Open();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    utilities.WriteLogError(IpAddr, ex);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                utilities.WriteLogError(IpAddr, ex);
                return false;
            }
        }
    }
}