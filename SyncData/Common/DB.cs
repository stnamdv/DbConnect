using log4net;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SyncData.Common
{
    public class DB
    {
        private OracleConnection conn;
        private string connectionString = string.Empty;
        static OracleCommand cmd = null;
        OracleTransaction transaction;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Open()
        {
            conn = new OracleConnection(connectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public DataTable ExecuteDataTable(CommandType commandType, string commandText, OracleParameter[] commandParameters, ref string errMsg)
        {
            try
            {
                Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = commandType;
                cmd.CommandText = commandText;
                cmd.CommandTimeout = 0;
                if (commandParameters != null)
                {
                    cmd.Parameters.AddRange(commandParameters);
                }
                //cmd.ExecuteNonQuery();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                DataTable dt = ds.Tables["table"];
                da.Dispose();

                return dt;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Log.FatalFormat("ExecuteDataTable threw an exception at: {1} - Log message: {2}", commandText, ex);
                return null;
            }
            finally
            {
                Close();
            }
        }

        public void ExecuteNonQuery(CommandType commandType, string commandText, OracleParameter[] commandParameters, ref string errMsg)
        {
            try
            {
                Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = commandType;
                cmd.CommandText = commandText;
                cmd.CommandTimeout = 0;

                if (commandParameters != null)
                {
                    cmd.Parameters.AddRange(commandParameters);
                }
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Log.FatalFormat("ExecuteNonQuery threw an exception at: {1} - Log message: {2}", commandText, ex);
            }
            finally
            {
                Close();
            }
        }

        public object ExecuteScalar(CommandType commandType, string commandText, OracleParameter[] commandParameters, ref string errMsg)
        {
            try
            {
                Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = commandType;
                cmd.CommandText = commandText;
                cmd.CommandTimeout = 0;

                if (commandParameters != null)
                {
                    cmd.Parameters.AddRange(commandParameters);
                }
                object r = cmd.ExecuteScalar();

                return r;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Log.FatalFormat("ExecuteScalar threw an exception at: {1} - Log message: {2}", commandText, ex);
                return null;
            }
            finally
            {
                Close();
            }
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText, OracleParameter[] commandParameters, ref string errMsg)
        {
            try
            {
                Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = commandType;
                cmd.CommandText = commandText;
                cmd.CommandTimeout = 0;

                if (commandParameters != null)
                {
                    cmd.Parameters.AddRange(commandParameters);
                }
                //cmd.ExecuteNonQuery();
                IDataReader dr = (IDataReader)cmd.ExecuteReader();

                return dr;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Log.FatalFormat("ExecuteReader threw an exception at: {1} - Log message: {2}", commandText, ex);
                return null;
            }
        }

        public void BulkInsert(string destTableName, DataTable dt, ref string errMsg)
        {
            try
            {
                Open();
                using (var bulkCopy = new OracleBulkCopy(conn))
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName.ToUpper(), col.ColumnName.ToUpper());
                    }
                    transaction = conn.BeginTransaction();
                    bulkCopy.DestinationTableName = destTableName;
                    bulkCopy.BulkCopyTimeout = 1800;
                    bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Log.FatalFormat("BulkInsert threw an exception at: {1} - Log message: {2}", destTableName, ex);
            }
            finally
            {
                transaction.Dispose();
                Close();
            }
        }

        internal DataTable ExecuteDataTable(CommandType text, string sSQL, object p)
        {
            throw new NotImplementedException();
        }
    }
}