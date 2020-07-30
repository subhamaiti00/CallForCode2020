using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

    public class ConnectionS1
    {
        public SqlConnection getConnection()
        {

            // SqlConnection con = new SqlConnection(@"Server=subha-PC\SQLEXPRESS;Database=WBMSCL_Payroll1;Integrated Security=True;");
            //SqlConnection con = new SqlConnection(@"Data Source=.;database=WBMSCL_Payroll;uid=sa;pwd=dmrudra");
            //SqlConnection con = new SqlConnection(@"Data Source=.;database=WBMSCL_Payroll;uid=sa;pwd=infixia100");
            //SqlConnection con = new SqlConnection(@"Data Source=103.48.51.87,1232;database=WBMSCL_Payroll;uid=sa;pwd=YJ6r7fZ#E^8doBj*m");
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conS1"].ConnectionString);
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            return con;
        }
    }

