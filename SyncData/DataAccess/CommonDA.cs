using System.Diagnostics;
using System.Net;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using log4net;
using SyncData.Models;
using SyncData.Common;

namespace SyncData.DataAccess
{
    public class CommonDA
    {
        private DatabaseInfo objDb;
        private UtilitiesFunc objFunc = new UtilitiesFunc();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string ipAddr = string.Empty;

        public CommonDA(DatabaseInfo _objDb = null)
        {
            objDb = _objDb;
            ipAddr = objFunc.GetIPAddress();
        }

        public object res = null;
        public object ExecuteSQL(string sql, string actionName, ref string errMsg)
        {
            try
            {
                var db = new DB(objDb.CONNECTIONSTRING);
                Log.InfoFormat("Try to execute: {0}, IP: {1}", sql, ipAddr);

                if (actionName == GlobalStruct.ActionType.ExecuteNonQuery)
                {
                    db.ExecuteNonQuery(CommandType.Text, sql, null, ref errMsg);
                }
                else if (actionName == GlobalStruct.ActionType.ExecuteReader)
                {
                    res = db.ExecuteReader(CommandType.Text, sql, null, ref errMsg);
                }
                else if (actionName == GlobalStruct.ActionType.ExecuteScalar)
                {
                    res = db.ExecuteScalar(CommandType.Text, sql, null, ref errMsg);
                }
                else if (actionName == GlobalStruct.ActionType.ExecuteDataTable)
                {
                    res = db.ExecuteDataTable(CommandType.Text, sql, null, ref errMsg);
                }

                Log.InfoFormat("Finished: {0}, IP: {1}", sql, ipAddr);
                return res;
            }
            catch (Exception ex)
            {
                Log.FatalFormat("Execute {0}, fail: {1}, IP: {2}", sql, ex, ipAddr);
                errMsg = ex.Message;
                return null;
            }
        }
    }
}