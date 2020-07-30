using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Data.DB2.Core;
using IBM.Watson.Assistant.v2;
using IBM.Watson.Assistant.v2.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CallForCodeApi.Model
{
    public class DatabaseContext : DbContext
    {

        //public DbSet<Users> Users { get; set; }
        //private readonly ILogger<DatabaseContext> _logger;
        //public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger) : base(options)
        //{
        //    _logger = logger;
        //}

        //string connectionString = "Server=dashdb-txn-sbox-yp-lon02-07.services.eu-gb.bluemix.net:50001;Database=BLUDB;UID=wxg76063;PWD=2+rrdsjwr2t51532;Security=SSL;";

        #region Watson...
        string apikey = "fbEjdZMnOd3VHBLzxmk3HNjhU85fhNGZPna1XqfIshYA";
        string url = "https://api.eu-gb.assistant.watson.cloud.ibm.com/instances/45dda911-4aa4-4b20-9339-d9a165beaf36";
        string versionDate = "16/07/2020";
        string assistantId = "cf37ceec-1e2f-4438-85d5-29e8c3f2c9ae";
        string sessionId;
        string inputString = "hello";


        #region Sessions
        public void CreateSession()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2020-04-01", authenticator);
            service.SetServiceUrl(url);

            var result = service.CreateSession(
                assistantId: assistantId
                );

            Console.WriteLine(result.Response);

            sessionId = result.Result.SessionId;
        }

        public void DeleteSession()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2020-04-01", authenticator);
            service.SetServiceUrl(url);

            var result = service.DeleteSession(
                assistantId: assistantId,
                sessionId: sessionId
                );

            Console.WriteLine(result.Response);
        }
        #endregion

        #region Message
        public string Message(string text)
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2020-04-01", authenticator);
            service.SetServiceUrl(url);

            var result = service.Message(
                assistantId: assistantId,
                sessionId: sessionId,
                input: new MessageInput()
                {
                    Text = text
                }
                );

            Console.WriteLine(result.Response);

            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result.Response);
            return myDeserializedClass.output.generic[0].text.ToString();
        }
        #endregion

        public class Intent
        {
            public string intent { get; set; }
            public Int64 confidence { get; set; }

        }
        public class Generic
        {
            public string response_type { get; set; }
            public string text { get; set; }
        }
        public class Output
        {
            public List<Intent> intents { get; set; }
            public List<object> entities { get; set; }
            public List<Generic> generic { get; set; }
        }
        public class Root
        {
            public Output output { get; set; }
        }
        #endregion..

        //#region Api
        //public string Login(string email, string pwd)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "SELECT (CAST(ID AS varchar(50)) || '~' || UType || '~' || Uname || '~'||email || '~' ||Mobile) role" +
        //                          " FROM USERS where email ='" + email + "' and pwd = '" + pwd + "' and isdeleted = 0;" +
        //                          "Update USERS SET LastLoggedIn=CURRENT_DATE where email ='" + email + "' and pwd = '" + pwd + "' and isdeleted = 0;";
        //        cmd.CommandText = command;
        //        cmd.Parameters.Add(new DB2Parameter("email", email));
        //        cmd.Parameters.Add(new DB2Parameter("pwd", pwd));
        //        conn.Open();
        //        var result = cmd.ExecuteScalar();
        //        conn.Close();
        //        if (result != null)
        //        {
        //            sJSON = result.ToString();
        //        }
        //        if (!string.IsNullOrEmpty(sJSON))
        //            sJSON = "{\"uid\":\"" + sJSON.Split('~')[0] + "\",\"role\":\"" + sJSON.Split('~')[1].ToLower() + "\",\"name\":\"" + sJSON.Split('~')[2].ToLower() + "\",\"email\":\"" + sJSON.Split('~')[3] + "\",\"mob\":\"" + sJSON.Split('~')[4] + "\"}";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string Register(Users user)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "RegisterSP";
        //        cmd.Parameters.Add(new DB2Parameter("UName", user.UNAME));
        //        cmd.Parameters.Add(new DB2Parameter("ADDRESS", user.ADDRESS));
        //        cmd.Parameters.Add(new DB2Parameter("EMAIL", user.EMAIL));
        //        cmd.Parameters.Add(new DB2Parameter("PWD", user.PWD));
        //        cmd.Parameters.Add(new DB2Parameter("MOBILE", user.MOBILE));
        //        cmd.Parameters.Add(new DB2Parameter("Country", user.Country));
        //        cmd.Parameters.Add(new DB2Parameter("State", user.State));
        //        cmd.Parameters.Add(new DB2Parameter("City", user.City));
        //        cmd.Parameters.Add(new DB2Parameter("Dist", user.Dist));
        //        cmd.Parameters.Add(new DB2Parameter("Pin", user.Pin));
        //        cmd.Parameters.Add(new DB2Parameter("Occupation", user.Occupation));
        //        cmd.Parameters.Add(new DB2Parameter("HealthProblem", user.HealthProblem));
        //        cmd.Parameters.Add(new DB2Parameter("BplIdCard", user.BplIdCard));
        //        cmd.Parameters.Add(new DB2Parameter("Photo", user.Photo));
        //        conn.Open();
        //        var result = cmd.ExecuteNonQuery();
        //        if (result > 0)
        //            sJSON = "1";
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string Categories()
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "select ID,Cname,CImg from CategoryMaster where IsDeleted=0";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Category> lstCategory = new List<Category>();
        //        Category objCategory;
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //            {
        //                objCategory = new Category();
        //                objCategory.ID = dr["ID"].ToString();
        //                objCategory.Cname = dr["Cname"].ToString();
        //                objCategory.CImg = dr["CImg"].ToString();

        //                lstCategory.Add(objCategory);
        //            }
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstCategory);
        //        }
        //        // sJSON = "Hello World";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string SubCategories(int catId)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "select ID, SubCatName, Simg from SubCatMaster where IsDeleted = 0 and CategoryMasterID = '" + catId + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Category> lstCategory = new List<Category>();
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //                lstCategory.Add(new Category()
        //                {
        //                    ID = dr["ID"].ToString(),
        //                    Cname = dr["SubCatName"].ToString(),
        //                    CImg = dr["Simg"].ToString()
        //                });
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstCategory);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}


        //public string Products(int catId)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "select * from Products where IsDeleted=0 and SubCategoryMasterID='" + catId + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Product> productList1 = new List<Product>();
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //                productList1.Add(new Product()
        //                {
        //                    ID = dr["ID"].ToString(),
        //                    Pname = dr["Pname"].ToString(),
        //                    Pimg = dr["Pimg"].ToString(),
        //                    PDesc = dr["PDesc"].ToString(),
        //                    PShortDesc = dr["PShortDesc"].ToString(),
        //                    Price = dr["Price"].ToString()
        //                });
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(productList1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string GetProducts(int pid, int uid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = string.Empty;
        //        if (pid == 0)
        //            command = @"select C.ID,p.Pname,(p.Price*C.Qty) as Price,P.Pimg,C.Qty,P.PShortDesc,P.ID as PID from Products P join CartDetails C on P.ID=C.ProductID 
        //                    where P.IsDeleted=0 and C.CustID=" + uid;
        //        else
        //            command = "select p.ID,p.Pname,Price,P.Pimg,1 as Qty,P.PShortDesc,P.ID as PID from Products p where ID='" + pid + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Product> lstProduct = new List<Product>();
        //        Product objProduct;
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //            {
        //                objProduct = new Product();
        //                objProduct.ID = dr["ID"].ToString();
        //                objProduct.Pname = dr["Pname"].ToString();
        //                objProduct.Pimg = dr["Pimg"].ToString();

        //                objProduct.PShortDesc = dr["PShortDesc"].ToString();
        //                objProduct.Price = dr["Price"].ToString();
        //                objProduct.Qty = dr["Qty"].ToString();
        //                objProduct.PID = dr["PID"].ToString();
        //                lstProduct.Add(objProduct);
        //            }
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstProduct);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string AddToCart(int pid, int uid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = string.Empty;
        //        command = @"
        //            MERGE INTO CartDetails AS t
        //            USING (select * from CartDetails WHERE CustID='" + uid + "' and ProductID= '" + pid + "') as s " +
        //               " on t.id=s.id " +
        //              " WHEN MATCHED " +
        //               " THEN  update set t.qty=s.Qty+1 " +
        //           " WHEN NOT MATCHED " +
        //                " THEN  insert (CustID,ProductID,AddedOn,Qty) values(s.CustID,s.ProductID,current_date,s.Qty);";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var result = cmd.ExecuteNonQuery();
        //        if (result > 0)
        //            sJSON = "1";
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string ViewCart(int uid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = string.Empty;
        //        command = @"select C.ID,p.Pname,(p.Price*C.Qty) as Price,P.Pimg,C.Qty,P.PShortDesc,P.ID as PID from Products P join CartDetails C on P.ID=C.ProductID 
        //                    where P.IsDeleted=0 and C.CustID=" + uid;
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Product> lstProduct = new List<Product>();
        //        Product objProduct;
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //            {
        //                objProduct = new Product();
        //                objProduct.ID = dr["ID"].ToString();
        //                objProduct.Pname = dr["Pname"].ToString();
        //                objProduct.Pimg = dr["Pimg"].ToString();

        //                objProduct.PShortDesc = dr["PShortDesc"].ToString();
        //                objProduct.Price = dr["Price"].ToString();
        //                objProduct.Qty = dr["Qty"].ToString();
        //                objProduct.PID = dr["PID"].ToString();
        //                lstProduct.Add(objProduct);
        //            }
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstProduct);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string RemoveCart(int cartid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = string.Empty;
        //        command = "delete from CartDetails where Id=" + cartid;
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var result = cmd.ExecuteNonQuery();
        //        if (result > 0)
        //            sJSON = "1";
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string ViewOrder(string uid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "select OD.ID,p.Pname,(OD.Price*OD.Qty) Price,P.Pimg,OD.Qty,P.PShortDesc,P.ID PID,OD.sts from Products P join OrderDetails OD on P.ID=OD.ProductID join OrderMaster OM on OD.OrderMasterID=OM.ID where P.IsDeleted=0 and OM.CustID='" + uid + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Product> productList4 = new List<Product>();
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //                productList4.Add(new Product()
        //                {
        //                    ID = dr["ID"].ToString(),
        //                    Pname = dr["Pname"].ToString(),
        //                    Pimg = dr["Pimg"].ToString(),
        //                    PShortDesc = dr["PShortDesc"].ToString(),
        //                    Price = dr["Price"].ToString(),
        //                    Qty = dr["Qty"].ToString(),
        //                    PID = dr["PID"].ToString(),
        //                    OrderSts = dr["sts"].ToString()
        //                });
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(productList4);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string NewOrderAdmin()
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = @"SELECT O.ID
        //                  ,CustID
        //               ,u.UName
        //                  ,OrderValue                          
        //                  ,DATE
        //                  ,Remarks
        //                  ,OrderNo
        //                  ,PayMode
        //                  ,AdvAmt
        //                  ,AdvType_Half_Full
        //                  ,PaymentID
        //                  ,O.Address
        //                  ,O.City
        //                  ,O.Pin
        //                  ,O.LandMark,Mobile
        //              FROM OrderMaster O join Users u on U.ID=O.CustID where O.Sts='OrderReceived'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Order> lstOrders = new List<Order>();
        //        Order objOrder;
        //        if (dr != null)
        //        {

        //            while (dr.Read())
        //            {
        //                objOrder = new Order();
        //                objOrder.ID = Convert.ToInt32(dr["ID"]);
        //                objOrder.CustID = Convert.ToInt32(dr["CustID"]);
        //                objOrder.UName = dr["UName"].ToString();
        //                objOrder.OrderValue = Convert.ToDecimal(dr["OrderValue"]);
        //                objOrder.Date = dr["Date"].ToString();
        //                objOrder.Remarks = dr["Remarks"].ToString();
        //                objOrder.OrderNo = dr["OrderNo"].ToString();
        //                objOrder.PayMode = dr["PayMode"].ToString();
        //                objOrder.AdvAmt = Convert.ToDecimal(dr["AdvAmt"]);
        //                objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
        //                objOrder.PaymentID = dr["PaymentID"].ToString();
        //                objOrder.Address = dr["Address"].ToString();
        //                objOrder.City = dr["City"].ToString();
        //                objOrder.Pin = dr["Pin"].ToString();
        //                objOrder.LandMark = dr["LandMark"].ToString();
        //                objOrder.Mobile = dr["Mobile"].ToString();
        //                lstOrders.Add(objOrder);
        //            }
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstOrders);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string AssignedOrderAdmin()
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = @"SELECT O.ID
        //                  ,CustID
        //               ,u.UName
        //                  ,OrderValue
        //                  ,Date
        //                  ,Remarks
        //                  ,OrderNo
        //                  ,PayMode
        //                  ,AdvAmt
        //                  ,AdvType_Half_Full
        //                  ,PaymentID
        //                  ,O.Address
        //                  ,O.City
        //                  ,O.Pin
        //                  ,O.LandMark
        //                  ,A.SE_Names
        //                  ,A.FromDate From
        //,A.ToDate To
        //              FROM OrderMaster O join Users u on U.ID=O.CustID 
        //                          join AssignmentMaster A on A.OrderMasterID=O.ID
        //				  where O.Sts='Assigned' order by FromDate desc";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Order> lstOrders = new List<Order>();
        //        Order objOrder;
        //        if (dr != null)
        //        {

        //            while (dr.Read())
        //            {
        //                objOrder = new Order();
        //                objOrder.ID = Convert.ToInt32(dr["ID"]);
        //                objOrder.CustID = Convert.ToInt32(dr["CustID"]);
        //                objOrder.UName = dr["UName"].ToString();
        //                objOrder.OrderValue = Convert.ToDecimal(dr["OrderValue"]);
        //                objOrder.Date = dr["Date"].ToString();
        //                objOrder.Remarks = dr["Remarks"].ToString();
        //                objOrder.OrderNo = dr["OrderNo"].ToString();
        //                objOrder.PayMode = dr["PayMode"].ToString();
        //                objOrder.AdvAmt = Convert.ToDecimal(dr["AdvAmt"]);
        //                objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
        //                objOrder.PaymentID = dr["PaymentID"].ToString();
        //                objOrder.Address = dr["Address"].ToString();
        //                objOrder.City = dr["City"].ToString();
        //                objOrder.Pin = dr["Pin"].ToString();
        //                objOrder.LandMark = dr["LandMark"].ToString();

        //                objOrder.SE_Names = dr["SE_Names"].ToString();
        //                objOrder.From = dr["From"].ToString();
        //                objOrder.To = dr["To"].ToString();
        //                lstOrders.Add(objOrder);
        //            }
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstOrders);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string ViewOrderAdmin(string uid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = "select OD.ID,p.Pname,(OD.Price*OD.Qty) Price,P.Pimg,OD.Qty,P.PShortDesc,P.ID PID,OD.sts from Products P join OrderDetails OD on P.ID=OD.ProductID join OrderMaster OM on OD.OrderMasterID=OM.ID where P.IsDeleted=0 and OM.CustID='" + uid + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();

        //        List<Product> productList4 = new List<Product>();
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //                productList4.Add(new Product()
        //                {
        //                    ID = dr["ID"].ToString(),
        //                    Pname = dr["Pname"].ToString(),
        //                    Pimg = dr["Pimg"].ToString(),
        //                    PShortDesc = dr["PShortDesc"].ToString(),
        //                    Price = dr["Price"].ToString(),
        //                    Qty = dr["Qty"].ToString(),
        //                    PID = dr["PID"].ToString(),
        //                    OrderSts = dr["sts"].ToString()
        //                });
        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(productList4);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //public string SeHome(string seid)
        //{
        //    string sJSON = string.Empty;
        //    try
        //    {
        //        DB2Connection conn = new DB2Connection(connectionString);
        //        DB2Command cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        string command = @"SELECT O.ID
        //                        ,CustID
        //                     ,u.UName
        //                        ,OrderValue
        //                        ,Date
        //                        ,O.Remarks
        //                        ,OrderNo
        //                        ,PayMode
        //                        ,AdvAmt
        //                        ,AdvType_Half_Full
        //                        ,PaymentID
        //                        ,O.Address
        //                        ,O.City
        //                        ,O.Pin
        //                        ,O.LandMark
        //                        FROM OrderMaster O join Users u on U.ID=O.CustID
        //                     join OrderAssignment OA on OA.OrderMasterID=O.ID
        //                  where O.Sts in ('Assigned','InProgress') and OA.SE_ID='" + seid + "'";
        //        cmd.CommandText = command;
        //        conn.Open();
        //        var dr = cmd.ExecuteReader();
        //        List<Order> lstOrders = new List<Order>();
        //        if (dr != null)
        //        {
        //            lstOrders = new List<Order>();
        //            Order objOrder = new Order();
        //            while (dr.Read())
        //                objOrder.ID = Convert.ToInt32(dr["ID"]);
        //            objOrder.CustID = Convert.ToInt32(dr["CustID"]);
        //            objOrder.UName = dr["UName"].ToString();
        //            objOrder.OrderValue = Convert.ToDecimal(dr["OrderValue"]);
        //            objOrder.Date = dr["Date"].ToString();
        //            objOrder.Remarks = dr["Remarks"].ToString();
        //            objOrder.OrderNo = dr["OrderNo"].ToString();
        //            objOrder.PayMode = dr["PayMode"].ToString();
        //            objOrder.AdvAmt = Convert.ToDecimal(dr["AdvAmt"]);
        //            objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
        //            objOrder.PaymentID = dr["PaymentID"].ToString();
        //            objOrder.Address = dr["Address"].ToString();
        //            objOrder.City = dr["City"].ToString();
        //            objOrder.Pin = dr["Pin"].ToString();
        //            objOrder.LandMark = dr["LandMark"].ToString();

        //            objOrder.SE_Names = dr["SE_Names"].ToString();
        //            objOrder.From = dr["From"].ToString();
        //            objOrder.To = dr["To"].ToString();
        //            lstOrders.Add(objOrder);

        //            conn.Close();
        //            sJSON = JsonConvert.SerializeObject(lstOrders);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return sJSON;
        //}

        //#endregion Api
    }
}
