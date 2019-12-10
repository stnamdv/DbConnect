using SyncData.Common;
using SyncData.DataAccess;
using SyncData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyncData.Controllers
{
    public class QueryController : Controller
    {
        private DatabaseInfo obj;
        private DatabaseInfoCtrl objCtrl = new DatabaseInfoCtrl();
        private CommonDA objDa;
        private string errMsg;
        private UtilitiesFunc objFunc = new UtilitiesFunc();

        // GET: Query
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadDb()
        {
            List<DatabaseInfo> lst = new List<DatabaseInfo>();
            obj = objFunc.GetDbSessionSrc();
            if (obj != null)
            {
                lst.Add(obj);
            }

            obj = objFunc.GetDbSessionDes();
            if (obj != null)
            {
                lst.Add(obj);
            }

            string res = lst.ToSerializer();
            return Json(res);
        }

        [HttpPost]
        public ActionResult Execute(string query, string dbActive)
        {
            if (dbActive == "1")
            {
                obj = objFunc.GetDbSessionSrc();
            }
            else
            {
                obj = objFunc.GetDbSessionDes();
            }

            objDa = new CommonDA(obj);
            DataTable dt = objDa.ExecuteSQL(query, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(errMsg);
            }
            List<Dictionary<string, object>> lst = dt.ToListDictionary();
            string res = lst.ToSerializer();

            return Json(res);
        }
    }
}