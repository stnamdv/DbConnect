using SyncData.Common;
using SyncData.DataAccess;
using SyncData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SyncData.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseInfo obj;
        private DatabaseInfoCtrl objCtrl = new DatabaseInfoCtrl();
        private CommonDA objDa;
        private string errMsg;
        private UtilitiesFunc objFunc = new UtilitiesFunc();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact.";

            return View();
        }

        [HttpPost]
        public ActionResult TestConnect(DatabaseInfo _obj)
        {
            bool r = objCtrl.TestConnect(_obj, ref errMsg);
            if (r)
            {
                return Json("Test connect successfully!");
            }
            return Json(errMsg);
        }

        [HttpPost]
        public ActionResult Connect(DatabaseInfo _obj)
        {
            bool r = objCtrl.TestConnect(_obj, ref errMsg);
            if (r)
            {
                objFunc.SetDbSession(_obj);
                return Json("1");
            }
            return Json(errMsg);
        }

        [HttpPost]
        public ActionResult CheckSession(string type)
        {
            if (type == "1")//source
            {
                DatabaseInfo _obj = objFunc.GetDbSessionSrc();
                if (_obj != null)
                {
                    return Json("0");
                }
                return Json("1");
            }
            else if (type == "2")//des
            {
                DatabaseInfo _obj = objFunc.GetDbSessionDes();
                if (_obj != null)
                {
                    return Json("0");
                }
                return Json("1");
            }
            return Json("1");
        }

        [HttpPost]
        public ActionResult LogOut(string type)
        {
            objFunc.ClearDbSession(type);
            return Json("1");
        }

        [HttpPost]
        public ActionResult LoadDb()
        {
            obj = objFunc.GetDbSessionSrc();
            objDa = new CommonDA(obj);
            string sql = "SELECT DISTINCT OBJECT_ID, OBJECT_NAME, OBJECT_TYPE, TO_CHAR(CREATED, 'DD/MM/YYYY HH24:MI:SS') CREATED_DATE, TEMPORARY " +
                "FROM USER_OBJECTS " +
                "WHERE OBJECT_TYPE = 'TABLE' ORDER BY OBJECT_NAME";
            DataTable dt = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            List<ObjectMapping> _lst = dt.ToListObject<ObjectMapping>();
            string res = _lst.ToSerializer();

            return Json(res);
        }

        [HttpPost]
        public ActionResult LoadTableDetail(string tableName)
        {
            obj = objFunc.GetDbSessionSrc();
            objDa = new CommonDA(obj);
            string sql = "SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + tableName + "' ORDER BY COLUMN_NAME";
            DataTable dt = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            List<ColumnMapping> _lst = dt.ToListObject<ColumnMapping>();
            string res = _lst.ToSerializer();

            return Json(res);
        }

        [HttpPost]
        public ActionResult LoadDbDes()
        {
            obj = objFunc.GetDbSessionDes();
            objDa = new CommonDA(obj);
            string sql = "SELECT DISTINCT OBJECT_ID, OBJECT_NAME, OBJECT_TYPE, TO_CHAR(CREATED, 'DD/MM/YYYY HH24:MI:SS') CREATED_DATE, TEMPORARY " +
                "FROM USER_OBJECTS " +
                "WHERE OBJECT_TYPE = 'TABLE' ORDER BY OBJECT_NAME";
            DataTable dt = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            List<ObjectMapping> _lst = dt.ToListObject<ObjectMapping>();
            string res = _lst.ToSerializer();

            return Json(res);
        }

        [HttpPost]
        public ActionResult LoadTableDetailDes(string tableName)
        {
            obj = objFunc.GetDbSessionDes();
            objDa = new CommonDA(obj);
            string sql = "SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + tableName + "' ORDER BY COLUMN_NAME";
            DataTable dt = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            List<ColumnMapping> _lst = dt.ToListObject<ColumnMapping>();
            string res = _lst.ToSerializer();

            return Json(res);
        }

        [HttpPost]
        public ActionResult CountTotal(string tableName, string dbActive)
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
            string sql = "SELECT COUNT(1) FROM " + tableName;
            object r = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteScalar, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(errMsg);
            }
            string total = string.Format("{0:n0}", r.ToInt32());
            string res = "Total rows of table " + tableName + ": " + total;
            return Json(res);
        }

        [HttpPost]
        public ActionResult Sync(DbSync _objDb)
        {
            obj = objFunc.GetDbSessionSrc();
            if (obj == null)
            {
                return Json("Please check your connection to source database!");
            }

            objDa = new CommonDA(obj);
            string where = string.Empty;
            if (!string.IsNullOrEmpty(_objDb.WHERE))
            {
                where += " WHERE ";
                where += _objDb.WHERE;
            }

            string sql = "SELECT " + _objDb.SOURCE_COLUMN + " FROM " + _objDb.DB_SOURCE + where;
            DataTable dt = objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteDataTable, ref errMsg).ToDataTable();
            //string[] _columnNames = dt.Columns.Cast<DataColumn>()
            //                     .Select(x => x.ColumnName)
            //                     .ToArray();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(errMsg);
            }
            int rowCount = dt.Rows.Count;
            if (rowCount == 0)
            {
                return Json("Table does not contain any item!");
            }

            sql = string.Empty;
            string sqlSync = string.Empty;
            string sqlInsert = string.Empty;

            var colName = _objDb.SOURCE_COLUMN.Split(',');
            sql += "DECLARE BLOBVAL RAW(32767); BEGIN ";
            string blobVal = string.Empty;
            foreach (DataRow item in dt.Rows)
            {
                sqlSync += " UPDATE " + _objDb.DB_DESTINATION + " SET ";
                sqlInsert += "INSERT INTO " + _objDb.DB_DESTINATION + "(" + _objDb.SOURCE_COLUMN + ") SELECT ";
                foreach (string cName in colName)
                {
                    sqlSync += cName + " = " + GetColValueByName(item, cName, ref blobVal) + ",";
                    sqlInsert += GetColValueByName(item, cName, ref blobVal) + ",";
                }
                sqlSync = sqlSync.Remove(sqlSync.Length - 1);
                sqlInsert = sqlInsert.Remove(sqlInsert.Length - 1);

                sqlSync += " WHERE " + GetWhereByKey(item, _objDb.KEY.Split(',')) + " 1 = 1; ";
                sqlInsert += " FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM " + _objDb.DB_DESTINATION + " WHERE " + GetWhereByKey(item, _objDb.KEY.Split(',')) + " 1 = 1); ";

                sqlSync += sqlInsert;

                sql += blobVal + sqlSync;
            }
            sql += " END;";

            obj = objFunc.GetDbSessionDes();
            if (obj == null)
            {
                return Json("Please check your connection to destination database!");
            }

            objDa = new CommonDA(obj);
            objDa.ExecuteSQL(sql, GlobalStruct.ActionType.ExecuteNonQuery, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Json(errMsg);
            }

            return Json(rowCount.ToString() + " items has been synchronized!");
        }

        private string GetColValueByName(DataRow dr, string colName, ref string blobVal)
        {
            object r = dr.Field<object>(colName);
            if (r != null)
            {
                if (r.GetType() == typeof(DateTime))
                {
                    string _r = "TO_DATE('" + r + "', 'MM/DD/YYYY HH24:MI:SS')";
                    return _r;
                }
                else if (r.GetType() == typeof(byte[]))
                {
                    string _blob = (r as byte[]).ToBase64String();
                    blobVal = "BLOBVAL := Utl_Raw.cast_to_raw('" + _blob + "');";
                    string _r = "BLOBVAL";
                    //_r = "";

                    return _r;
                }
                else
                {
                    return "'" + r.ToSpaceString().Trim() + "'";
                }
            }

            return "''";
        }

        private string GetWhereByKey(DataRow dr, string[] key)
        {
            object r;
            string res = string.Empty;
            foreach (string item in key)
            {
                r = dr.Field<object>(item);
                if (r != null)
                {
                    if (r.GetType() == typeof(DateTime))
                    {
                        string _r = item + " = " + "TO_DATE('" + r.ToSpaceString().Trim() + "', 'MM/DD/YYYY HH24:MI:SS') AND ";
                        res += _r;
                    }
                    //else if (r.GetType() == typeof(byte[]))
                    //{
                    //    string _blob = (r as byte[]).ToBase64String();
                    //    _r = item + " = " + "TO_BLOB('" + _blob + "') AND ";

                    //    res += _r;
                    //}
                    else
                    {
                        string _r = item + " = '" + r.ToSpaceString().Trim() + "' AND ";
                        res += _r;
                    }
                }
            }

            return res;
        }
    }
}