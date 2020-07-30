using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;//
using System.Data;//
using System.Globalization;
using System.Drawing;//
using System.IO;


    public class DataAccessS1 : IDisposable
    {
        #region Data Access Member Variables
        private DataTable dt;
        private SqlDataAdapter da;
        private SqlConnection con;
        private SqlCommand cmd;
        SqlParameterCollection spc;
        private DataRow dr;
        ConnectionS1 conn = new ConnectionS1();
        List<SqlParameter> ListSqlParams = new List<SqlParameter>();
        #endregion

        #region Own Default Constructor
        public DataAccessS1()
        {


        }


        #endregion

        #region Data Access Member Functions
        public void addParam(string name, object val)
        {
            SqlParameter p = new SqlParameter(name, val);
            ListSqlParams.Add(p);
        }
        public DataTable returnTable(string sql)
        {
            try
            {
                con = conn.getConnection();
                dt = new DataTable();
                da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return dt;
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet returnDataset(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                con = conn.getConnection();

                da = new SqlDataAdapter(sql, con);
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable returnTable(string sql, out string ErrMsg)
        {
            try
            {
                con = conn.getConnection();
                dt = new DataTable();
                da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                ErrMsg = "SUCCESS";
                return dt;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return dt;
            }
            finally
            {
                con.Close();
            }
        }

        public string returnString(string sql)
        {
            try
            {
                con = conn.getConnection();
                dt = new DataTable();
                da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    return dr[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public string returnString(string sql, out string ErrMsg)
        {
            try
            {
                con = conn.getConnection();
                dt = new DataTable();
                da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    ErrMsg = "SUCCESS";
                    return dr[0].ToString();
                }
                else
                {
                    ErrMsg = "NO_RESULT_RETURNED";
                    return "";
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public Int32 returnInt32(string sql)
        {
            Int32 i = -1;
            try
            {
                con = conn.getConnection();
                cmd = new SqlCommand(sql, con);
                con.Open();
                i = (Int32)cmd.ExecuteScalar();
                return i;
            }
            catch
            {
                return i;
            }
            finally
            {
                con.Close();
            }
        }

        public Int32 returnInt32(string sql, out string ErrMsg)
        {
            Int32 i = -1;
            try
            {
                con = conn.getConnection();
                cmd = new SqlCommand(sql, con);
                con.Open();
                i = (Int32)cmd.ExecuteScalar();
                ErrMsg = "SUCCESS";
                return i;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return i;
            }
            finally
            {
                con.Close();
            }
        }

        public decimal returnDecimal(string sql)
        {
            decimal d = -1;
            try
            {
                con = conn.getConnection();
                cmd = new SqlCommand(sql, con);
                con.Open();
                d = decimal.Parse(cmd.ExecuteScalar().ToString());
                return d;
            }
            catch
            {
                return d;
            }
            finally
            {
                con.Close();
            }
        }

        public decimal returnDecimal(string sql, out string ErrMsg)
        {
            decimal d = -1;
            try
            {
                con = conn.getConnection();
                cmd = new SqlCommand(sql, con);
                con.Open();
                d = (decimal)cmd.ExecuteScalar();
                ErrMsg = "SUCCESS";
                return d;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return d;
            }
            finally
            {
                con.Close();
            }
        }

        public int executeTQuery(SqlCommand command)
        {
            int i = -1;
            try
            {
                i = command.ExecuteNonQuery();
                return i;
            }
            catch
            {
                return i;
            }
        }

        public int executeTQuery(SqlCommand command, out string ErrMsg)
        {
            int i = -1;
            try
            {
                i = command.ExecuteNonQuery();
                ErrMsg = "SUCCESS";
                return i;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return i;
            }
        }

        public int executeTQuery(string sql)
        {
            int i = -1;
            try
            {
                con = conn.getConnection();
                con.Open();
                cmd = new SqlCommand(sql, con);
                i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
            finally
            {
                con.Close();
            }
        }

        public int executeTQuery(string sql, out string ErrMsg)
        {
            int i = -1;
            try
            {
                con = conn.getConnection();
                con.Open();
                cmd = new SqlCommand(sql, con);
                i = cmd.ExecuteNonQuery();
                ErrMsg = "SUCCESS";
                if (i == 0)
                    ErrMsg = "No Rows Effected.";
                return i;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return i;
            }
            finally
            {
                con.Close();
            }
        }

        public int executeTQueryEx(string sql)
        {
            int i = -1;
            try
            {
                con = conn.getConnection();
                con.Open();
                cmd = new SqlCommand(sql, con);
                i = Convert.ToInt32(cmd.ExecuteScalar());
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
            finally
            {
                con.Close();
            }
        }

        //public void fillCombo(MultiColumnComboBox cmb, string sql, string displayMember, string valueMember)
        //{
        //    try
        //    {
        //        DataTable table = new DataTable();
        //        table = returnTable(sql);
        //        cmb.DataSource = table;
        //        cmb.DisplayMember = displayMember;
        //        cmb.ValueMember = valueMember;
        //    }
        //    catch
        //    { }

        //}

        //public void fillCombo(MultiColumnComboBox cmb, string sql, string displayMember, string valueMember, out string ErrMsg)
        //{
        //    try
        //    {
        //        DataTable table = new DataTable();
        //        table = returnTable(sql);
        //        cmb.DataSource = table;
        //        cmb.DisplayMember = displayMember;
        //        cmb.ValueMember = valueMember;
        //        ErrMsg = "SUCCESS";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrMsg = ex.Message;
        //    }

        //}


        public DateTime stringToDate(string date)
        {
            DateTime ob = DateTime.Now.Date;
            try
            {
                string[] arr = date.Split('/');
                ob = new DateTime(int.Parse(arr[2]), int.Parse(arr[1]), int.Parse(arr[0]));

            }
            catch { }
            return ob;
        }

        public DateTime stringToDate(string strDate, string specifiedFormat)
        {
            IFormatProvider cultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime date = DateTime.ParseExact(strDate, specifiedFormat, cultureInfo);
            return date;
        }

        public DateTime stringToDate(string strDate, string specifiedFormat, string separator)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = specifiedFormat;
            dtfi.DateSeparator = separator;
            DateTime date = Convert.ToDateTime(@strDate, dtfi);
            return date;
        }

        public DataTable getDataTable(string NameSP)
        {

            SqlDataAdapter sqlDataAdapter;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter;
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection();
            int iListCount = ListSqlParams.Count;
            int iCount;
            if (iListCount > 0)
            {

                for (iCount = 0; iCount < iListCount; iCount++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[iCount];
                    sqlCommand.Parameters.Add(sqlParameter);
                }

                try
                {

                    sqlConnection.ConnectionString = conn.getConnection().ConnectionString;
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.CommandText = NameSP;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dt);
                    sqlConnection.Close();
                }

                catch (Exception E)
                {
                    throw;
                }

                finally
                {
                    if (sqlConnection.State != ConnectionState.Closed)
                    {
                        sqlConnection.Close();
                    }

                }

            }

            return (dt);
        }

        public DataTable getDataTableWithoutPara(string NameSP)
        {

            SqlDataAdapter sqlDataAdapter;
            SqlCommand sqlCommand = new SqlCommand();
            //SqlParameter sqlParameter;
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection();
            //int iListCount = ListSqlParams.Count;
            //int iCount;
            //if (iListCount > 0)
            //{

            //for (iCount = 0; iCount < iListCount; iCount++)
            //{
            //    sqlParameter = new SqlParameter();
            //    sqlParameter = ListSqlParams[iCount];
            //    sqlCommand.Parameters.Add(sqlParameter);
            //}

            try
            {

                sqlConnection.ConnectionString = conn.getConnection().ConnectionString;
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                sqlCommand.CommandText = NameSP;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dt);
                sqlConnection.Close();
            }

            catch (Exception E)
            {
                throw;
            }

            finally
            {
                if (sqlConnection.State != ConnectionState.Closed)
                {
                    sqlConnection.Close();
                }

            }

            //}

            return (dt);
        }

        public object ExecuteScalerSP(string NameSP)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter;
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection();
            int iListCount = ListSqlParams.Count;
            int iCount;
            object obj = new object();
            if (iListCount > 0)
            {
                for (iCount = 0; iCount < iListCount; iCount++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[iCount];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                try
                {

                    sqlConnection.ConnectionString = conn.getConnection().ConnectionString;
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.CommandText = NameSP;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    obj = sqlCommand.ExecuteScalar();
                    sqlConnection.Close();

                }

                catch (Exception E)
                {
                    throw;
                }

                finally
                {
                    if (sqlConnection.State != ConnectionState.Closed)
                    {
                        sqlConnection.Close();
                    }

                }
                return obj;

            }

            return (dt);
        }
        public int ExecuteQuerySP(string NameSP)
        {
            int rt = 0;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter;
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection();
            int iListCount = ListSqlParams.Count;
            int iCount;
            object obj = new object();
            if (iListCount > 0)
            {
                for (iCount = 0; iCount < iListCount; iCount++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[iCount];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                try
                {

                    sqlConnection.ConnectionString = conn.getConnection().ConnectionString;
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.CommandText = NameSP;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    rt = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                }

                catch (Exception E)
                {
                    
                }

                finally
                {
                    if (sqlConnection.State != ConnectionState.Closed)
                    {
                        sqlConnection.Close();
                    }

                }
                return rt;

            }

            return (rt);
        }
        public SqlDataReader ExecuteReader(string CommandText, CommandType CmdType)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand sqlCommand;
            sqlCommand = new SqlCommand();
             SqlParameter sqlParameter;           
            SqlConnection sqlConnection = new SqlConnection();
            int iListCount = ListSqlParams.Count;
            int iCount;           
            SqlDataReader dr ;
            
                for (iCount = 0; iCount < iListCount; iCount++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[iCount];
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                con = conn.getConnection();
                sqlCommand.Connection = con;
                sqlCommand.CommandText = CommandText;
                sqlCommand.CommandType = CmdType;
                
                con.Open();
                try
                {
                    dr = sqlCommand.ExecuteReader();
                }
                finally
                {
                   // con.Close();
                }
            
            return dr;
        }

        public string QuotReplace(string str)
        {
            string name = str;
            if (name.Contains("''"))
                name = name.Replace("''", "''''");
            else if (name.Contains("'"))
                name = name.Replace("'", "''");

            return name;
        }

        public string ValidDecimal(string str)
        {
            try
            {
                decimal d = decimal.Parse(str);

                if (!str.Contains("."))
                {
                    if (str.Length == 0)
                        str = "0.00";
                    else if (str.Length > 0)
                        str += ".00";
                }
                else
                {
                    string[] arr = str.Split('.');
                    if (arr[1].Length == 0)
                    {
                        str = arr[0] + ".00";
                    }
                    else if (arr[1].Length == 1)
                    {
                        try
                        {
                            d = decimal.Parse(arr[1]);
                            str = arr[0] + "." + arr[1] + "0";
                        }
                        catch { str = arr[0] + ".00"; }
                    }
                    else if (arr[1].Length == 2)
                    {
                        try
                        {
                            d = decimal.Parse(arr[1]);
                            str = arr[0] + "." + arr[1];
                        }
                        catch { str = arr[0] + ".00"; }
                    }
                    else if (arr[1].Length > 2)
                    {
                        try
                        {
                            d = decimal.Parse(arr[1]);
                            str = arr[0] + "." + arr[1].Substring(0, 2);
                        }
                        catch { str = arr[0] + ".00"; }
                    }
                }
            }
            catch { str = "0.00"; }

            return str;
        }

        public Color NextColor()
        {
            string hexcode = "#" + System.Guid.NewGuid().ToString().Substring(0, 6);
            Color c = new Color();
            c = ColorTranslator.FromHtml(hexcode);
            return c;
        }
        public Color NextColor(int rVal, int gVal, int bVal)
        {
            Color c = new Color();
            Random rnd = new Random();
            c = Color.FromArgb(rVal, gVal, bVal);
            return c;
        }




        /// <summary>
        /// Make DataGridView Columns Visible on flagIsVisible value.
        /// </summary>
        /// <param name="dgv">Reference DataGridView</param>
        /// <param name="inVisCol">Reference DataGridView Columns Name</param>
        /// <param name="flagIsVisible">Visible Flag : True For Visible &amp; False For Hide</param>        


        public bool checkExistance(string sql)
        {
            int i = returnInt32(sql);
            if (i != -1)
            {
                if (i > 0)
                    return true;
            }
            return false;
        }


        #endregion
        
        public string GetAppSettings(string Key)
        {

            try
            {
                con = conn.getConnection();
                dt = new DataTable();
                da = new SqlDataAdapter("select AppSettingsValue from AppSettings where AppKey='"+ Key + "'", con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    
                    return dr[0].ToString();
                }
                else
                {
                    
                    return "";
                }
            }
            catch (Exception ex)
            {
               
                return "";
            }
            finally
            {
                con.Close();
            }
    }
        public Byte[] ImageToByteArray(Image img)
        {
            try
            {
                MemoryStream mstImage = new MemoryStream();
                img.Save(mstImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                Byte[] bytImage = mstImage.GetBuffer();
                return bytImage;
            }
            catch
            {
                // do something smart
                return null;
            }
        }

        #region Check Db Exist
        public static bool IsDBExists(string dbServer, string dbName, string dbUid, string dbPass)
        {
            string sql;
            bool result = false;
            string str;
            str = @"server=" + dbServer + ";initial catalog=" + dbName + ";uid=" + dbUid + ";pwd=" + dbPass + "";

            try
            {

                SqlConnection con = new SqlConnection("server=" + dbServer + ";Trusted_Connection=yes");


                sql = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", dbName);

                using (con)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        int databaseID = (int)sqlCmd.ExecuteScalar();
                        con.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch //(Exception ex)
            {
                result = false;
            }

            return result;
        }
        #endregion



        #region Dictionary Find By Key & By Value
        /// <summary>
        /// Returns value of dictionary object by its key.
        /// </summary>
        /// <typeparam name="TKey">Define Dictionary's Key type.</typeparam>
        /// <typeparam name="TValue">Define Dictionary's Value type.</typeparam>
        /// <param name="dictionary">Dictionary object to be passed.</param>
        /// <param name="value">Searching word within dictionary.</param>
        /// <returns>Returns result as object.Cast according to your usage.</returns>
        public object FindKeyByValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("Dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (value.Equals(pair.Value))
                {
                    return pair.Key;
                }
            }
            return null;
        }
        public object FindValueByKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey Key)
        {
            if (dictionary == null)
                throw new ArgumentNullException("Dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (Key.Equals(pair.Key))
                {
                    return pair.Value;
                }
            }
            return null;
        }
        #endregion



        void IDisposable.Dispose()
        {

        }
    }
    

