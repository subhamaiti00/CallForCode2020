using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CallForCodeApi.Model
{
    public class Helper
    {
        //public static DataTable ConvertCSVtoDataTable(string strFilePath)
        //{
        //    DataTable dt = new DataTable();
           
        //    using (StreamReader sr = new StreamReader(strFilePath))
        //    {
        //        string[] headers = sr.ReadLine().Split(',');
        //        foreach (string header in headers)
        //        {
        //            dt.Columns.Add(header);
        //        }
        //        while (!sr.EndOfStream)
        //        {
        //            string[] rows = sr.ReadLine().Split(',');
        //            DataRow dr = dt.NewRow();
        //            for (int i = 0; i < headers.Length; i++)
        //            {
        //                dr[i] = rows[i];
        //            }
        //            dt.Rows.Add(dr);
        //        }

        //    }


        //    return dt;
        //}

        public static  DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        public static async  Task<DataTable> HttpGetForLargeFileInRightWay(string url)
        {
            DataTable dt = new DataTable();
            using (HttpClient client = new HttpClient())
            {
                
                //url = "http://callforcodeapi.abahan.com//S1/callforcode_updated.csv";
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (StreamReader sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }

                }
                return dt;
            }
        }
    }
}
