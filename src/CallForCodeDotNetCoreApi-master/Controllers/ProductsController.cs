using CallForCodeApi.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CallForCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private IWebHostEnvironment _env;
        public ProductsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("Products")]
        public IActionResult Products(int scatid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=Products&scatid=" + scatid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts(int pid, int uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=getproducts&uid=" + uid + "&pid=" + pid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("Productsbkp")]
        //public IActionResult Productsbkp(int catid)
        //{
        //    string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=Productsbkp&catid='" + catid + "'";
        //    string html = RequestApi.GetApi(url);
        //    return Ok(html);
        //}
        [HttpGet]
        [Route("ProductRecom")]
        public IActionResult ProductRecom(int uid)
        {
            try
            {
                string[] product_cols = { "CUSTID", "Detergent Powder", "Dettol", "Geometry Sets", "Handwash", "Note Book", "Pen", "Pencil", "Rechargeble Solor Light", "Regular Rice 10 kg", "Solar Cooker", "Glucose solution", "Cerelac From 6 Months", "Cerelac From 12 Months", "Rice 5 kg", "Milk Cereal", "OZiva Protein", "Veg Soup", "Horlicks" };
                //var webRoot = _env.ContentRootPath;
                //var filePath = webRoot+ "\\Data\\customers_orders1_opt_updated.csv";
                // DataTable df = Helper.ConvertCSVtoDataTable("http://callforcodeapi.abahan.com//S1/callforcode_updated.csv");
                //DataTable df_filtered = df.Select(["CUST_ID"] + product_cols);
                var filePath = "http://callforcodeapi.abahan.com//S1/customers_orders1_opt_updated.csv";
                DataTable df = Helper.HttpGetForLargeFileInRightWay(filePath).Result;
                System.Data.DataView view = new System.Data.DataView(df);
                System.Data.DataTable df_filtered =
                        view.ToTable("Selected", false, product_cols);
                DataTable df_customer_products = df_filtered.AsEnumerable()
                          .GroupBy(r => r.Field<string>("CUSTID"))
                          .Select(g =>
                          {
                              var row = df_filtered.NewRow();

                              row["CUSTID"] = g.Key;
                              row["Detergent Powder"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Detergent Powder")));
                              row["Dettol"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Dettol")));
                              row["Geometry Sets"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Geometry Sets")));
                              row["Handwash"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Handwash")));
                          //row["Baby wash"] = g.Sum(r =>Convert.ToInt32( r.Field<string>("Baby wash")));
                          row["Note Book"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Note Book")));
                              row["Pen"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Pen")));
                              row["Pencil"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Pencil")));
                              row["Rechargeble Solor Light"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Rechargeble Solor Light")));
                              row["Regular Rice 10 kg"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Regular Rice 10 kg")));
                              row["Solar Cooker"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Solar Cooker")));
                              row["Glucose solution"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Glucose solution")));
                              row["Cerelac From 6 Months"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Cerelac From 6 Months")));
                              row["Cerelac From 12 Months"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Cerelac From 12 Months")));
                              row["Rice 5 kg"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Rice 5 kg")));
                              row["Milk Cereal"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Milk Cereal")));
                              row["Veg Soup"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Veg Soup")));
                              row["Horlicks"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Horlicks")));

                              return row;
                          }).CopyToDataTable();
                //use caching to avoid loop
                DataTable df_customer_products_cluster = df_customer_products.Copy();
                df_customer_products_cluster.Columns.Add("cluster", typeof(int));
                df_customer_products_cluster.AcceptChanges();
                foreach (DataRow r in df_customer_products_cluster.Rows)
                {
                    r["cluster"] = new Random().Next(1, 50);
                }

                string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=viewcart&uid=" + uid;
                string html = RequestApi.GetApi(url);
                // DataTable dt = Helper.JsonStringToDataTable(html);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(html, (typeof(DataTable)));
                string fields = "", values = "";
                string cols = "";
                int cnt = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    fields = fields + "\"sum(" + dr["Pname"].ToString() + ")\",";
                    cols += dr["Pname"].ToString() + ",";
                    values += Convert.ToInt16(dr["Qty"].ToString()).ToString() + ",";
                    cnt++;
                }
                if (cols == "")
                {
                    cols = "Dettol,";
                }
                foreach (string col in product_cols)
                {
                    if (!fields.Contains(col))
                    {
                        fields = fields + "\"sum(" + col + ")\",";
                        values += "0,";
                        cnt++;
                    }
                }
                fields = fields.Remove(fields.Length - 1, 1);
                values = values.Remove(values.Length - 1, 1);
                //string scoring_payload = "{\"fields\":[\"sum(Baby Food)\",\"sum(Diapers)\",\"sum(Formula)\",\"sum(Lotion)\",\"sum(Baby wash)\",\"sum(Wipes)\",\"sum(Fresh Fruits)\",\"sum(Fresh Vegetables)\",\"sum(Beer)\",\"sum(Wine)\",\"sum(Club Soda)\",\"sum(Sports Drink)\",\"sum(Chips)\",\"sum(Popcorn)\",\"sum(Oatmeal)\",\"sum(Medicines)\",\"sum(Canned Foods)\",\"sum(Cigarettes)\",\"sum(Cheese)\",\"sum(Cleaning Products)\",\"sum(Condiments)\",\"sum(Frozen Foods)\",\"sum(Kitchen Items)\",\"sum(Meat)\",\"sum(Office Supplies)\",\"sum(Personal Care)\",\"sum(Pet Supplies)\",\"sum(Sea Food)\",\"sum(Spices)\"],\"values\":[[1,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]]}";
                string scoring_payload = "{\"fields\":[" + fields + "],\"values\":[[" + values + "]]}";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                var request = (HttpWebRequest)WebRequest.Create("https://iam.bluemix.net/identity/token");

                var postData = "apikey=CFdFMUPI3YBwFUMPcjVSXUmNvnJp1OEiNDzT0VG79yX2&grant_type=urn:ibm:params:oauth:grant-type:apikey";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var json = JsonConvert.DeserializeObject<dynamic>(responseString);
                string access_token = json["access_token"];

                request = (HttpWebRequest)WebRequest.Create("https://eu-gb.ml.cloud.ibm.com/v3/wml_instances/da7bd924-fe24-41c2-a6d9-a5259a586ad5/deployments/c15fb9d5-78cc-44db-9119-8e8981d6b1b6/online");


                data = Encoding.ASCII.GetBytes(scoring_payload);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + access_token);
                request.Headers.Add("ML-Instance-ID", "da7bd924-fe24-41c2-a6d9-a5259a586ad5");
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.DeserializeObject<dynamic>(responseString);

                int cluster = (int)(json["values"][0][cnt + 1]);
                DataRow[] df_cluster_products = df_customer_products_cluster.Select("cluster=" + cluster.ToString());
                if (df_cluster_products.Length == 0)
                    df_cluster_products = df_customer_products_cluster.Select("" + product_cols[2] + ">0");
                DataTable df_cluster_products_agg = df_cluster_products.AsEnumerable()
                          .GroupBy(r => r.Field<int>("cluster"))
                          .Select(g =>
                          {
                              var row = df_filtered.NewRow();

                          //row["cluster"] = g.Key;
                          row["Detergent Powder"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Detergent Powder")));
                              row["Dettol"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Dettol")));
                              row["Geometry Sets"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Geometry Sets")));

                              row["Handwash"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Handwash")));
                              row["Note Book"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Note Book")));
                              row["Pen"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Pen")));

                              row["Pencil"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Pencil")));
                              row["Rechargeble Solor Light"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Rechargeble Solor Light")));
                              row["Regular Rice 10 kg"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Regular Rice 10 kg")));

                              row["Solar Cooker"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Solar Cooker")));
                              row["Glucose solution"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Glucose solution")));
                              row["Cerelac From 6 Months"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Cerelac From 6 Months")));

                              row["Cerelac From 12 Months"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Cerelac From 12 Months")));
                              row["Rice 5 kg"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Rice 5 kg")));
                              row["Milk Cereal"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Milk Cereal")));

                              row["Veg Soup"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Veg Soup")));
                              row["Horlicks"] = g.Sum(r => Convert.ToInt32(r.Field<string>("Horlicks")));


                              return row;
                          }).CopyToDataTable();
                DataRow row_ = df_cluster_products_agg.Rows[0];
                Dictionary<string, int> products_acnt = new Dictionary<string, int>();
                df_cluster_products_agg.Columns.Remove("CUSTID");
                df_cluster_products_agg.AcceptChanges();

                var productList1 = new List<Product>();
                string url1 = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=Products";
                string html1 = RequestApi.GetApi(url1);
                //DataTable dt1 = Helper.JsonStringToDataTable(html1);
                DataTable dt1 = (DataTable)JsonConvert.DeserializeObject(html1, (typeof(DataTable)));
                foreach (DataRow dr in dt1.Rows)
                {
                    foreach (DataColumn dc in df_cluster_products_agg.Columns)
                    {
                        if (dr["Pname"].ToString() == dc.ColumnName)
                            if (!string.IsNullOrEmpty(Convert.ToString(row_[dc.ColumnName])))
                            {
                                if (Convert.ToInt32(row_[dc.ColumnName].ToString()) > 0)
                                    productList1.Add(new Product()
                                    {
                                        ID = dr["ID"].ToString(),
                                        Pname = dr["Pname"].ToString(),
                                        Pimg = dr["Pimg"].ToString(),
                                        PDesc = dr["PDesc"].ToString(),
                                        PShortDesc = dr["PShortDesc"].ToString(),
                                        Price = dr["Price"].ToString()
                                    });

                            }
                    }
                }

                return Ok(productList1);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
