using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

namespace Archid
{
    public class DataManagerS1 : IDisposable
    {
        private readonly string strConnectionString;
        private CommandType mCommandType;
        private SqlCommand SqlCmd;

        public DataManagerS1()
        { 
            SQLExecutionEvent = null;
            mCommandType = CommandType.StoredProcedure;
            SqlCmd = new SqlCommand();
            SqlCmd.CommandTimeout = 1000;
            strConnectionString = ConfigurationManager.ConnectionStrings["conS1"].ConnectionString;
            //if (HttpContext.Current.Session == null)
            //{
            //    DataTable dtPortalSettings = GetPortalSettingscommon();
            //    if (dtPortalSettings.Rows.Count > 0)
            //        strConnectionString = dtPortalSettings.Rows[0]["connectionString"].ToString();
            //}
            //else if(HttpContext.Current.Session["ConnectionString"]!=null)
            //    strConnectionString = HttpContext.Current.Session["ConnectionString"].ToString();
            //else if (HttpContext.Current.Session["ConnectionString"] == null)
            //{
            //    DataTable dtPortalSettings = GetPortalSettingscommon();
            //    if (dtPortalSettings.Rows.Count > 0)
            //        strConnectionString = dtPortalSettings.Rows[0]["connectionString"].ToString();
            //}

            //XmlDocument oXmlDocument = new XmlDocument();
            //FileInfo fi = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/CRM/DataBaseConnection.xml"));
            //if (fi.Exists)
            //{
            //    oXmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/CRM/DataBaseConnection.xml"));
            //    for (int i = 0; i < oXmlDocument.LastChild.ChildNodes.Count; i++)
            //    {
            //        if (oXmlDocument.LastChild.ChildNodes[i].Attributes["Site"].Value == System.Web.HttpContext.Current.Request.Url.Host)
            //        {
            //            this.strConnectionString = oXmlDocument.LastChild.ChildNodes[i].Attributes["connectionString"].Value;
            //            break;
            //        }
            //    }
            //} 
        }
        public string GetConnectionString()
        {
            return strConnectionString;
        }
        public DataManagerS1(string strConnection)
        {
            SQLExecutionEvent = null;
            mCommandType = CommandType.StoredProcedure;
            SqlCmd = new SqlCommand();
            SqlCmd.CommandTimeout = 1000;
            strConnectionString = strConnection;
            //strConnectionString = ConfigurationManager.ConnectionStrings["CRMDataBaseConnection"].ConnectionString;

        }
        public DataManagerS1(string strConnection,string rpt)
        {
            SQLExecutionEvent = null;
            mCommandType = CommandType.StoredProcedure;
            SqlCmd = new SqlCommand();
            SqlCmd.CommandTimeout = 1000;
            strConnectionString = strConnection;

        }
        public CommandType CommandType
        {
            get { return mCommandType; }
            set { mCommandType = value; }
        }

        public int Count
        {
            get { return SqlCmd.Parameters.Count; }
        }

        public SqlParameter this[int index]
        {
            get { return SqlCmd.Parameters[index]; }
            set { SqlCmd.Parameters[index] = value; }
        }

        public SqlParameter this[string parameterName]
        {
            get { return SqlCmd.Parameters[parameterName]; }
            set { SqlCmd.Parameters[parameterName] = value; }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion

        public event SQLExecution SQLExecutionEvent;

        public SqlParameter Add(SqlParameter SqlPar)
        {
            return SqlCmd.Parameters.Add(SqlPar);
        }

        private SqlParameter Add(SqlParameter SqlPar, ParameterDirection direction)
        {
            SqlPar.Direction = direction;
            return SqlCmd.Parameters.Add(SqlPar);
        }

        public SqlParameter Add(string parameterName, object value)
        {
            return Add(new SqlParameter(parameterName, value));
        }

        private SqlParameter Add(SqlParameter SqlPar, ParameterDirection direction, object value)
        {
            SqlPar.Value = value;
            SqlPar.Direction = direction;
            return SqlCmd.Parameters.Add(SqlPar);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, ParameterDirection direction)
        {
            return Add(new SqlParameter(parameterName, sqlDbType), direction);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, object value)
        {
            return Add(new SqlParameter(parameterName, sqlDbType), ParameterDirection.Input, value);
        }

        public SqlParameter Add(string parameterName, object value, ParameterDirection direction)
        {
            return Add(new SqlParameter(parameterName, value), direction);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, ParameterDirection direction, object value)
        {
            return Add(new SqlParameter(parameterName, sqlDbType), direction, value);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, ParameterDirection direction)
        {
            return Add(new SqlParameter(parameterName, sqlDbType, size), direction);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, object value)
        {
            return Add(new SqlParameter(parameterName, sqlDbType, size), ParameterDirection.Input, value);
        }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, ParameterDirection direction,
                                object value)
        {
            return Add(new SqlParameter(parameterName, sqlDbType, size), direction, value);
        }

        public void Clear()
        {
            SqlCmd.Parameters.Clear();
        }

        public bool Contains(object value)
        {
            return SqlCmd.Parameters.Contains(value);
        }

        public bool Contains(string value)
        {
            return SqlCmd.Parameters.Contains(value);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            SqlCmd.Parameters.Clear();
            if ((SqlCmd.Connection != null) && (SqlCmd.Connection.State == ConnectionState.Open))
            {
                try
                {
                    SqlCmd.Connection.Close();
                }
                catch
                {
                }
            }
            SqlCmd.Dispose();
        }

        public DataTable ExecuteDataTable(string ExecuteString)
        {
            using (SqlConnection connection = PrepareExecution(ExecuteString))
            {
                using (var adapter = new SqlDataAdapter(SqlCmd))
                {
                    var dataSet = new DataSet();
                    
                    adapter.Fill(dataSet);
                    if (dataSet.Tables.Count > 0)
                    {
                        return dataSet.Tables[0];
                    }
                }
            }
            return null;
        }

        public void ExecuteforEvent(string ExecuteString)
        {
            using (SqlConnection connection = PrepareExecution(ExecuteString))
            {
                if (SQLExecutionEvent != null)
                {
                    
                    using (SqlDataReader reader = SqlCmd.ExecuteReader())
                    {
                        SQLExecutionEvent(reader);
                    }
                }
            }
        }

        public void ExecuteforEvent(string ExecuteString, CommandBehavior behavior)
        {
            PrepareExecution(ExecuteString);
            if (SQLExecutionEvent != null)
            {
                using (SqlDataReader reader = SqlCmd.ExecuteReader(behavior))
                {
                    SQLExecutionEvent(reader);
                }
            }
        }

        public int ExecuteNonQuery(string ExecuteString)
        {
            using (SqlConnection connection = PrepareExecution(ExecuteString))
            {
                return SqlCmd.ExecuteNonQuery();
            }
        }

        public SqlDataReader ExecuteReader(string ExecuteString)
        {
            PrepareExecution(ExecuteString);
            return SqlCmd.ExecuteReader();
        }

        public SqlDataReader ExecuteReader(string ExecuteString, CommandBehavior behavior)
        {
            PrepareExecution(ExecuteString);
            return SqlCmd.ExecuteReader(behavior);
        }

        public object ExecuteScalar(string ExecuteString)
        {
            using (SqlConnection connection = PrepareExecution(ExecuteString))
            {
                return SqlCmd.ExecuteScalar();
            }
        }

        public XmlReader ExecuteXmlReader(string ExecuteString)
        {
            PrepareExecution(ExecuteString);
            return SqlCmd.ExecuteXmlReader();
        }

        ~DataManagerS1()
        {
            Dispose(false);
        }

        public IEnumerator GetEnumerator()
        {
            return SqlCmd.Parameters.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return SqlCmd.Parameters.IndexOf(value);
        }

        public int IndexOf(string parameterName)
        {
            return SqlCmd.Parameters.IndexOf(parameterName);
        }

        public void Insert(int index, object value)
        {
            SqlCmd.Parameters.Insert(index, value);
        }

        private SqlConnection PrepareExecution(string ExecuteString)
        {
            if (SqlCmd == null)
            {
                SqlCmd = new SqlCommand();
                SqlCmd.CommandTimeout = 1000;
            }
            if (SqlCmd.Connection == null)
            {
                SqlCmd.Connection = new SqlConnection(strConnectionString);
            }
            else if (SqlCmd.Connection.ConnectionString.Length == 0)
            {
                SqlCmd.Connection.ConnectionString = strConnectionString;
            }
            if (SqlCmd.Connection.State != ConnectionState.Open)
            {
                SqlCmd.Connection.Open();
            }
            if (SqlCmd.Parameters.Count > 0)
            {
                SqlCmd.CommandType = mCommandType;
            }
            else
            {
                SqlCmd.CommandType = mCommandType;
            }
            SqlCmd.CommandText = ExecuteString;
            return SqlCmd.Connection;
        }

        public void Remove(object value)
        {
            SqlCmd.Parameters.Remove(value);
        }

        public void RemoveAt(int index)
        {
            SqlCmd.Parameters.RemoveAt(index);
        }

        public void RemoveAt(string parameterName)
        {
            SqlCmd.Parameters.RemoveAt(parameterName);
        }

        public string GetURLcommon()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            string[] Url = url.Split('/');
            if (Url.Length >= 4)
                url = Url[0].ToString() + "//" + Url[2].ToString() + "/" + Url[3].ToString();
            return url;
        }

        public DataTable GetPortalSettingscommon()
        {
            DataTable PortalDetails = new DataTable();

            try
            {
                string PortalUrl = GetURLcommon();

                using (var dm = new DataManagerS1(ConfigurationManager.ConnectionStrings["DbConnection_zmec"].ConnectionString))
                {
                    dm.CommandType = CommandType.StoredProcedure;
                    dm.Add("@PortalURL", SqlDbType.VarChar, PortalUrl);
                    PortalDetails = dm.ExecuteDataTable("spGetPortalDetailsOnURL");
                }
            }
            catch (Exception ex)
            {
                // throw;
            }

            return PortalDetails;
        }
    }
}