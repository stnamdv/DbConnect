using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;

namespace SyncData.Common
{
    public static class CommonFunction
    {
        public static IDataReader ToDataReader(this object obj)
        {
            try
            {
                return obj as IDataReader;
            }
            catch
            {
                IDataReader idr = null;
                return idr;
            }
        }

        public static DataTable ToDataTable(this object obj)
        {
            try
            {
                return obj as DataTable;
            }
            catch
            {
                return null;
            }
        }

        public static string ToSpaceString(this object str)
        {
            try
            {
                return str.ToString();
            }
            catch
            {
                return " ";
            }
        }

        public static int ToInt32(this object str)
        {
            try
            {
                return Int32.Parse(str.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static double ToDouble(this object str)
        {
            try
            {
                return double.Parse(str.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static int? ToNullAbleInt32(this object str)
        {
            try
            {
                return Int32.Parse(str.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static long ToInt64(this object str)
        {
            try
            {
                return Int64.Parse(str.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static long ToUnixMillis(this string str)
        {
            try
            {
                DateTime dtInput = str.ToDateTime();
                //var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var diff = dtInput.ToUniversalTime().Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return (long)diff;
            }
            catch
            {
                return -1;
            }

        }

        public static long? ToNullAbleInt64(this object str)
        {
            try
            {
                return Int64.Parse(str.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static decimal ToDecimal(this object str)
        {
            try
            {
                return decimal.Parse(str.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static decimal? ToNullAbleDecimal(this object str)
        {
            try
            {
                return decimal.Parse(str.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static float ToFloat(this object str)
        {
            try
            {
                return float.Parse(str.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static string ToBase64String(this byte[] bytes)
        {
            try
            {
                return Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            catch
            {
                return "";
            }
        }

        public static float? ToNullAbleFloat(this object str)
        {
            try
            {
                return float.Parse(str.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static bool ToBoolean(this object str)
        {
            try
            {
                return str.ToString().ToLower() == "true" || str.ToString() == "1" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static bool? ToNullAbleBoolean(this object str)
        {
            try
            {
                if (str.ToString().ToLower() == "true" || str.ToString() == "1")
                {
                    return true;
                }
                if (str.ToString().ToLower() == "false" || str.ToString() == "0")
                {
                    return false;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDate(this string strDate, string strFormat = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.ParseExact(strDate, strFormat, null);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ToNullAbleDate(this string strDate, string strFormat = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.ParseExact(strDate, strFormat, null);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDateTime(this string strDate, string strFormat = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(strDate, strFormat, null, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ToNullAbleDateTime(this string strDate, string strFormat = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(strDate, strFormat, null);
            }
            catch
            {
                return null;
            }
        }

        public static string ToDateVN(this DateTime dtDate)
        {
            try
            {
                return dtDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }

        public static byte[] ToByte(this string str, Encoding encoding)
        {
            try
            {
                byte[] b = encoding.GetBytes(str);
                return b;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ToDateTimeVN(this DateTime dtDate)
        {
            try
            {
                return dtDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
                return "";
            }
        }

        public static string ToDateTimeVN(this long unixMillis)
        {
            try
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                origin = origin.AddMilliseconds(unixMillis).ToLocalTime();
                if (unixMillis == -1)
                    return "";
                return origin.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
                return "";
            }
        }

        public static string ToDateVN(this long unixMillis)
        {
            try
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                origin = origin.AddMilliseconds(unixMillis).ToLocalTime();
                if (unixMillis == -1)
                    return "";
                return origin.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }

        public static string ToTimeVN(this long unixMillis)
        {
            try
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                origin = origin.AddMilliseconds(unixMillis).ToLocalTime();
                if (unixMillis == -1)
                    return "";
                return origin.ToString("HH:mm:ss");
            }
            catch
            {
                return "";
            }
        }

        public static List<T> ToListObject<T>(this DataTable tbl) where T : new()
        {
            try
            {
                // define return list
                List<T> lst = new List<T>();

                // go through each row
                foreach (DataRow r in tbl.Rows)
                {
                    // add to the list
                    lst.Add(CreateItemFromRow<T>(r));
                }

                // return the list
                return lst;
            }
            catch
            {
                return new List<T>();
            }
        }

        public static T ToObject<T>(this DataTable tbl) where T : new()
        {
            try
            {
                T obj = new T();
                foreach (DataRow r in tbl.Rows)
                {
                    obj = CreateItemFromRow<T>(r);
                }
                return obj;
            }
            catch
            {
                return new T();
            }
        }

        public static T ToObjectByRow<T>(this DataRow r) where T : new()
        {
            try
            {
                T obj = new T();
                obj = CreateItemFromRow<T>(r);
                return obj;
            }
            catch
            {
                return new T();
            }
        }

        private static T CreateItemFromRow<T>(this DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        private static void SetItemFromRow<T>(this T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    try
                    {
                        p.SetValue(item, row[c].ToString(), null);
                    }
                    catch
                    {
                        p.SetValue(item, (byte[])row[c], null);
                    }
                }
            }
        }

        public static List<Dictionary<string, object>> ToListDictionary(this DataTable dt)
        {
            try
            {
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                return rows;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<OracleParameter> ToOracleParam<T>(this T item, string prefix = "P_") where T : class
        {
            try
            {
                List<OracleParameter> _prs = new List<OracleParameter>();
                foreach (PropertyInfo p in item.GetType().GetProperties())
                {
                    object _val = p.GetValue(item, null);
                    string _name = p.Name;
                    if (_val != null)
                    {
                        if (_val.GetType() == typeof(string) && _name == "XML")
                        {
                            _prs.Add(new OracleParameter(prefix + _name, OracleDbType.Clob, _val, ParameterDirection.Input));
                        }
                        else if (_val.GetType() == typeof(string))
                        {
                            _prs.Add(new OracleParameter(prefix + _name, _val));
                        }
                        else if (_val.GetType() == typeof(DateTime))
                        {
                            _prs.Add(new OracleParameter(prefix + _name, _val));
                        }
                        else if (_val.GetType() == typeof(byte[]))
                        {
                            _prs.Add(new OracleParameter(prefix + _name, OracleDbType.Blob, _val, ParameterDirection.Input));
                        }
                    }
                }
                return _prs;
            }
            catch
            {
                return new List<OracleParameter>();
            }
        }

        public static List<SqlParameter> ToSqlParam<T>(this T item) where T : new()
        {
            try
            {
                List<SqlParameter> _prs = new List<SqlParameter>();
                foreach (PropertyInfo p in item.GetType().GetProperties())
                {
                    object _val = p.GetValue(item, null);
                    string _name = p.Name;
                    if (_val != null)
                    {
                        _prs.Add(new SqlParameter("@" + _name, _val));
                    }
                }

                return _prs;
            }
            catch
            {
                return new List<SqlParameter>();
            }
        }

        public static T ToDeserialize<T>(this string context)
        {
            try
            {
                //cast to specified objectType
                var obj = (T)new JavaScriptSerializer().Deserialize<T>(context);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        public static string ToXML<T>(this List<T> item) where T : new()
        {
            try
            {
                string xml = string.Empty;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                var stringwriter = new StringWriter();
                serializer.Serialize(stringwriter, item, ns);
                xml = stringwriter.ToString();

                return xml;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ToSerializer(this object obj)
        {
            try
            {
                var js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                string res = js.Serialize(obj);
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ToCurrencyVN(this decimal s)
        {
            try
            {
                string rs = "";
                s = Math.Round(s, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = s.ToString();
                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }
                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }
                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng.";
                else
                    rs += "đồng.";
                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DismissAlert = "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"> <span aria-hidden=\"true\">&times;</span> </button>";

        public static string ToError(this string sMsg)
        {
            return string.Format("<div class=\"alert alert-warning\" role=\"alert\">{0}{1}</div>", sMsg, DismissAlert);
        }

        public static string ToSuccess(this string sMsg)
        {
            return string.Format("<div class=\"alert alert-success\" role=\"alert\">{0}{1}</div>", sMsg, DismissAlert);
        }

        public static string ToDanger(this string sMsg)
        {
            return string.Format("<div class=\"alert alert-danger\" role=\"alert\">{0}{1}</div>", sMsg, DismissAlert);
        }

        public static string ToPrimary(this string sMsg)
        {
            return string.Format("<div class=\"alert alert-info\" role=\"alert\">{0}{1}</div>", sMsg, DismissAlert);
        }

        public static string ToEncrypt(this string sMsg)
        {
            var md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var encoder = new System.Text.UTF8Encoding();
            string hashedBytes = BitConverter.ToString(md5Hasher.ComputeHash(encoder.GetBytes(sMsg)));
            return hashedBytes;
        }

        public static string ToStatus(this object str)
        {
            try
            {
                switch (str.ToString())
                {
                    case "1":
                        return "Hoạt động";
                    case "0":
                        return "Tạm khoá";
                    case "-9":
                        return "Đã xóa";
                    default:
                        return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }

        public static string ToStatusNotif(this object str)
        {
            try
            {
                switch (str.ToString())
                {
                    case "1":
                        return "<span class='fa fa-spin fa-spinner'></span> Đang xử lý";
                    case "0":
                        return "Thành công";
                    case "2":
                        return "Thất bại";
                    default:
                        return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }

        public static string ToStatusNotifClass(this object str)
        {
            try
            {
                switch (str.ToString())
                {
                    case "1":
                        return "text-info";
                    case "0":
                        return "text-success";
                    case "2":
                        return "text-danger";
                    default:
                        return "text-default";
                }
            }
            catch
            {
                return "text-default";
            }
        }

        public static string ToReadState(this object str)
        {
            try
            {
                if (str.ToString() == "0")
                    return "UnRead";
                return "Readed";

            }
            catch
            {
                return "";
            }
        }

        public static void ToCreateSuccess(this HtmlGenericControl divMsg)
        {
            divMsg.InnerHtml = "Thêm mới thành công!".ToSuccess();
        }

        public static void ToUpdateSuccess(this HtmlGenericControl divMsg)
        {
            divMsg.InnerHtml = "Cập nhật thành công!".ToSuccess();
        }

        public static void ToDeleteSuccess(this HtmlGenericControl divMsg)
        {
            divMsg.InnerHtml = "Xoá thành công!".ToSuccess();
        }

        public static string ToSessionID(this string sLink)
        {
            try
            {
                sLink = sLink.Replace("--", ".");
                var _link = sLink.Split('.');
                string r = _link[_link.Length - 2];

                return r;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}