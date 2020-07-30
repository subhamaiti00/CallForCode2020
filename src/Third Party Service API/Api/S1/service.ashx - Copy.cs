using Archid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Api.S1
{
    /// <summary>
    /// Summary description for service
    /// </summary>
    public class service : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string sql = "";
            string methodName = context.Request.QueryString["method"];
            System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.InputStream);
            string value = reader.ReadToEnd();
            DataAccessS1 dac = new DataAccessS1();
            SqlDataReader dr;
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer1;
            oSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
            //dynamic data;

            DataManagerS1 odem = new DataManagerS1();
            string sJSON = "", uid = "";

            switch (methodName)
            {
                case "login":
                    var ss = oSerializer1.Deserialize<dynamic>(value);
                    string email = ss["uid"];
                    string pwd = ss["pwd"];
                    string devInfo = context.Request.QueryString["devInfo"];
                    sJSON = "";
                    // sql = "select (cast(ID as varchar(50))+'~'+UType+'~'+Uname) [role] from dbo.Users where email ='" + email + "' and pwd='" + pwd + "' and isdeleted=0";
                    odem.Add("@email", email);
                    odem.Add("@pwd", pwd);
                    sJSON = Convert.ToString(odem.ExecuteScalar("LoginSP"));

                    if (!string.IsNullOrEmpty(sJSON))
                        //sJSON = "{\"uid\":\"" + sJSON.Split('~')[0] + "\",\"role\":\"" + sJSON.Split('~')[1].ToLower() + "\",\"name\":\"" + sJSON.Split('~')[2].ToLower() + "\"}";
                    sJSON= "{\"uid\":\"" + sJSON.Split('~')[0] + "\",\"role\":\"" + sJSON.Split('~')[1].ToLower() + "\",\"name\":\"" + sJSON.Split('~')[2].ToLower() + "\",\"email\":\"" + sJSON.Split('~')[3] + "\",\"mob\":\"" + sJSON.Split('~')[4] + "\"}";
                    //context.Response.ContentType = "application/text";
                    break;
                case "register":
                    ss = oSerializer1.Deserialize<dynamic>(value);
                    odem = new DataManagerS1();
                    odem.Add("@UName", ss["name"]);
                    odem.Add("@Address", ss["add"]);
                    odem.Add("@Email", ss["uid"]);
                    odem.Add("@Pwd", ss["pwd"]);
                    odem.Add("@Mobile", ss["mob"]);
                    odem.Add("@UType", "c");
                    odem.Add("@CreatedBy", 0);
                    try
                    {
                        if (odem.ExecuteNonQuery("RegisterSP") > 0)
                        {
                            sJSON = "1";
                        }
                    }
                    catch
                    {
                        sJSON = "";
                    }
                    //context.Response.ContentType = "application/text";
                    break;
                case "Categories":
                    dac = new DataAccessS1();
                    sql = "select ID,Cname,CImg from CategoryMaster where IsDeleted=0";
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    List<Category> lstCategory = new List<Category>();
                    Category objCategory;
                    while (dr.Read())
                    {
                        objCategory = new Category();
                        objCategory.ID = dr["ID"].ToString();
                        objCategory.Cname = dr["Cname"].ToString();
                        objCategory.CImg = dr["CImg"].ToString();

                        lstCategory.Add(objCategory);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstCategory);
                    break;
                case "SubCategories":
                    SqlDataReader sqlDataReader5 = new DataAccessS1().ExecuteReader("select ID,SubCatName,Simg from SubCatMaster where IsDeleted=0 and CategoryMasterID=" + context.Request.QueryString["catid"], CommandType.Text);
                    List<Category> categoryList2 = new List<Category>();
                    while (sqlDataReader5.Read())
                        categoryList2.Add(new Category()
                        {
                            ID = sqlDataReader5["ID"].ToString(),
                            Cname = sqlDataReader5["SubCatName"].ToString(),
                            CImg = sqlDataReader5["Simg"].ToString()
                        });
                    sqlDataReader5.Close();
                    sJSON = new JavaScriptSerializer().Serialize((object)categoryList2);
                    break;
                case "Productsbkp":
                    string catid = context.Request.QueryString["catid"];
                    dac = new DataAccessS1();
                    sql = "select * from Products where IsDeleted=0 and CategoryMasterID=" + catid;
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    List<Product> lstProduct = new List<Product>();
                    Product objProduct;
                    while (dr.Read())
                    {
                        objProduct = new Product();
                        objProduct.ID = dr["ID"].ToString();
                        objProduct.Pname = dr["Pname"].ToString();
                        objProduct.Pimg = dr["Pimg"].ToString();

                        objProduct.PDesc = dr["PDesc"].ToString();
                        objProduct.PShortDesc = dr["PShortDesc"].ToString();
                        objProduct.Price = dr["Price"].ToString();

                        lstProduct.Add(objProduct);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstProduct);
                    break;
                case "Products":
                    SqlDataReader sqlDataReader3 = new DataAccessS1().ExecuteReader("select * from Products where IsDeleted=0 and SubCategoryMasterID=" + context.Request.QueryString["scatid"], CommandType.Text);
                    List<Product> productList1 = new List<Product>();
                    while (sqlDataReader3.Read())
                        productList1.Add(new Product()
                        {
                            ID = sqlDataReader3["ID"].ToString(),
                            Pname = sqlDataReader3["Pname"].ToString(),
                            Pimg = sqlDataReader3["Pimg"].ToString(),
                            PDesc = sqlDataReader3["PDesc"].ToString(),
                            PShortDesc = sqlDataReader3["PShortDesc"].ToString(),
                            Price = sqlDataReader3["Price"].ToString()
                        });
                    sqlDataReader3.Close();
                    sJSON = new JavaScriptSerializer().Serialize((object)productList1);
                    break;
                case "addtocart":
                    uid = context.Request.QueryString["uid"];
                    string pid = context.Request.QueryString["pid"];
                    dac = new DataAccessS1();
                    dac.addParam("@custid", uid);
                    dac.addParam("@productid", pid);
                    if (dac.ExecuteQuerySP("spAddToCart") > 0)
                    {
                        sJSON = "1";
                    }
                    //string AmcID = context.Request.QueryString["AMCID"];
                    //string amcdetailid = context.Request.QueryString["AMCDetailID"];
                    //string Problem = context.Request.QueryString["Problem"];
                    break;
                case "viewcart":
                    uid = context.Request.QueryString["uid"];
                    dac = new DataAccessS1();
                    sql = @"select C.ID,p.Pname,(p.Price*C.Qty) [Price],P.Pimg,C.Qty,P.PShortDesc,P.ID [PID] from dbo.Products P join dbo.CartDetails C on P.ID=C.ProductID 
                            where P.IsDeleted=0 and C.CustID=" + uid;
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstProduct = new List<Product>();
                    //Product objProduct;
                    while (dr.Read())
                    {
                        objProduct = new Product();
                        objProduct.ID = dr["ID"].ToString();
                        objProduct.Pname = dr["Pname"].ToString();
                        objProduct.Pimg = dr["Pimg"].ToString();

                        objProduct.PShortDesc = dr["PShortDesc"].ToString();
                        objProduct.Price = dr["Price"].ToString();
                        objProduct.Qty = dr["Qty"].ToString();
                        objProduct.PID = dr["PID"].ToString();
                        lstProduct.Add(objProduct);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstProduct);
                    break;
                case "removefromcart":
                    uid = context.Request.QueryString["uid"];
                    string cartid = context.Request.QueryString["cartid"];
                    sql = "delete from dbo.CartDetails where Id=" + cartid;
                    if (dac.executeTQuery(sql) > 0)
                    {
                        sJSON = "1";
                    }
                    break;
                case "getproducts":
                    uid = context.Request.QueryString["uid"];
                    pid = context.Request.QueryString["pid"];
                    dac = new DataAccessS1();
                    if (pid == "0")
                        sql = @"select C.ID,p.Pname,(p.Price*C.Qty) [Price],P.Pimg,C.Qty,P.PShortDesc,P.ID [PID] from dbo.Products P join dbo.CartDetails C on P.ID=C.ProductID 
                            where P.IsDeleted=0 and C.CustID=" + uid;
                    else
                        sql = "select p.ID,p.Pname, [Price],P.Pimg,1 [Qty],P.PShortDesc,P.ID [PID] from dbo.Products p where ID=" + pid;
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstProduct = new List<Product>();
                    //Product objProduct;
                    while (dr.Read())
                    {
                        objProduct = new Product();
                        objProduct.ID = dr["ID"].ToString();
                        objProduct.Pname = dr["Pname"].ToString();
                        objProduct.Pimg = dr["Pimg"].ToString();

                        objProduct.PShortDesc = dr["PShortDesc"].ToString();
                        objProduct.Price = dr["Price"].ToString();
                        objProduct.Qty = dr["Qty"].ToString();
                        objProduct.PID = dr["PID"].ToString();
                        lstProduct.Add(objProduct);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstProduct);
                    break;
                case "Order":
                    string str3 = context.Request.QueryString["uid"];
                    string str4 = context.Request.QueryString["pid"];
                    string str5 = context.Request.QueryString["payAmt"];
                    string str6 = context.Request.QueryString["ActualAmt"];
                    string str7 = context.Request.QueryString["AdvAmt"];
                    string str8 = context.Request.QueryString["PaymentMode"];
                    string str9 = context.Request.QueryString["AdvType_Half_Full"];
                    string str10 = context.Request.QueryString["Qty"];
                    string str11 = context.Request.QueryString["paymentID"];
                    string NameSP = "SaveOrder";
                    DataAccessS1 dataAccessS1_2 = new DataAccessS1();
                    dataAccessS1_2.addParam("@uid", (object)str3);
                    dataAccessS1_2.addParam("@pid", (object)str4);
                    dataAccessS1_2.addParam("@ActualAmt", (object)str6);
                    dataAccessS1_2.addParam("@AdvAmt", (object)str7);
                    dataAccessS1_2.addParam("@PaymentMode", (object)str8);
                    dataAccessS1_2.addParam("@AdvType_Half_Full", (object)str9);
                    dataAccessS1_2.addParam("@Qty", (object)str10);
                    dataAccessS1_2.addParam("@paymentID", (object)str11);
                    dataAccessS1_2.addParam("@Address", context.Request.QueryString["address"]);
                    dataAccessS1_2.addParam("@City", context.Request.QueryString["city"]);
                    dataAccessS1_2.addParam("@Pin", context.Request.QueryString["pin"]);
                    dataAccessS1_2.addParam("@LandMark", context.Request.QueryString["landmark"]);
                    sJSON = "";
                    if (dataAccessS1_2.ExecuteQuerySP(NameSP) > 0)
                    {
                        sJSON = "1";
                        break;
                    }
                    break;
                case "vieworder":
                    SqlDataReader sqlDataReader10 = new DataAccessS1().ExecuteReader("select OD.ID,p.Pname,(OD.Price*OD.Qty) [Price],P.Pimg,OD.Qty,P.PShortDesc,P.ID [PID],OD.sts \r\n                            from dbo.Products P join dbo.OrderDetails OD on P.ID=OD.ProductID \r\n                                                join dbo.OrderMaster OM on OD.OrderMasterID=OM.ID\r\n                             where P.IsDeleted=0 and OM.CustID=" + context.Request.QueryString["uid"], CommandType.Text);
                    List<Product> productList4 = new List<Product>();
                    while (sqlDataReader10.Read())
                        productList4.Add(new Product()
                        {
                            ID = sqlDataReader10["ID"].ToString(),
                            Pname = sqlDataReader10["Pname"].ToString(),
                            Pimg = sqlDataReader10["Pimg"].ToString(),
                            PShortDesc = sqlDataReader10["PShortDesc"].ToString(),
                            Price = sqlDataReader10["Price"].ToString(),
                            Qty = sqlDataReader10["Qty"].ToString(),
                            PID = sqlDataReader10["PID"].ToString(),
                            OrderSts = sqlDataReader10["sts"].ToString()
                        });
                    sqlDataReader10.Close();
                    sJSON = new JavaScriptSerializer().Serialize((object)productList4);
                    break;
                case "NewOrderAdmin":
                    sql = @"SELECT O.[ID]
                          ,[CustID]
	                      ,u.UName
                          ,[OrderValue]
                          ,convert(varchar(10),[Date],103) [Date]
                          ,[Remarks]
                          ,[OrderNo]
                          ,[PayMode]
                          ,[AdvAmt]
                          ,[AdvType_Half_Full]
                          ,[PaymentID]
                          ,O.[Address]
                          ,[City]
                          ,[Pin]
                          ,[LandMark]
                      FROM [dbo].[OrderMaster] O join Users u on U.ID=O.CustID where O.Sts='OrderReceived'";
                    dac=new DataAccessS1
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstProduct = new List<Product>();
                    //Product objProduct;
                    while (dr.Read())
                    {
                        objProduct = new Product();
                        objProduct.ID = dr["ID"].ToString();
                        objProduct.Pname = dr["Pname"].ToString();
                        objProduct.Pimg = dr["Pimg"].ToString();

                        objProduct.PShortDesc = dr["PShortDesc"].ToString();
                        objProduct.Price = dr["Price"].ToString();
                        objProduct.Qty = dr["Qty"].ToString();
                        objProduct.PID = dr["PID"].ToString();
                        lstProduct.Add(objProduct);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstProduct);
                    break;
                case "PendingCallListSE":
                    uid = context.Request.QueryString["uid"];
                    string sts = context.Request.QueryString["sts"];
                    sql = "GetAssignedCallSEStatuswise";
                    dac = new DataAccessS1();
                    dac.addParam("@SEID", uid);
                    dac.addParam("@Status", sts);
                    dr = dac.ExecuteReader(sql, CommandType.StoredProcedure);
                    List<CallList> lstCall = new List<CallList>();
                    CallList objCallList;
                    while (dr.Read())
                    {
                        objCallList = new CallList();
                        objCallList.ExpectedDate = dr["ExpectedDate"].ToString();
                        objCallList.Party = dr["Party"].ToString();
                        objCallList.Address = dr["Address"].ToString();
                        objCallList.Mobile1 = dr["Mobile1"].ToString();
                        objCallList.Product = dr["Product"].ToString();
                        objCallList.DistributionID = dr["DistributionID"].ToString();
                        objCallList.Problem = dr["Problem"].ToString();
                        objCallList.AMCDetailID = dr["AMCDetailID"].ToString();
                        lstCall.Add(objCallList);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstCall);
                    break;
                case "ProductsPartyWise":
                    uid = context.Request.QueryString["uid"];
                    sql = "GetProductsPartyWise";
                    dac = new DataAccessS1();
                    dac.addParam("@PartyID", uid);
                    dr = dac.ExecuteReader(sql, CommandType.StoredProcedure);
                    lstCall = new List<CallList>();
                    while (dr.Read())
                    {
                        objCallList = new CallList();
                        //objCallList.Party = dr["Party"].ToString();                       
                        objCallList.Product = dr["Product"].ToString();
                        objCallList.EndDate = dr["EndDate"].ToString();
                        objCallList.ProductSlNo = dr["ProductSlNo"].ToString();
                        objCallList.AMCDetailID = dr["AMCDetailID"].ToString();
                        objCallList.AMCID = dr["AMCID"].ToString();
                        lstCall.Add(objCallList);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstCall);
                    break;
                case "partycallbooking":
                    uid = context.Request.QueryString["uid"];
                    string AmcID = context.Request.QueryString["AMCID"];
                    string amcdetailid = context.Request.QueryString["AMCDetailID"];
                    string Problem = context.Request.QueryString["Problem"];
                    sql = string.Format(@"INSERT INTO [dbo].[CallBooking]
                                       ([AMCID]
                                       ,[AMCDetailID]
                                       ,[DueDate]
                                       ,[Problem]
                                       ,[ExpectedDate])
                                        VALUES({0},{1},'{2}','{3}','{4}')", AmcID, amcdetailid, DateTime.Now.ToString("yyyyMMdd"), Problem.Replace("'", "''"), DateTime.Now.AddDays(20).ToString("yyyyMMdd"));
                    dac.executeTQueryEx(sql);
                    sJSON = "1";
                    break;
                case "updateCall":
                    uid = context.Request.QueryString["uid"];
                    sts = context.Request.QueryString["sts"];
                    string distID = context.Request.QueryString["distID"];
                    string rem = context.Request.QueryString["remarks"];
                    string completed = context.Request.QueryString["completed"];
                    string OTP = context.Request.QueryString["OTP"];
                    string AMCDetailID = context.Request.QueryString["AMCDetailID"];
                    if (sts == "C")
                    {
                        if (dac.returnString("select OTP from AMCDetails where AMCDetailID=" + AMCDetailID) != OTP)
                        {
                            sJSON = "0";
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(distID))
                    {
                        sql = "updateCallBookingAndDist";
                        dac = new DataAccessS1();
                        dac.addParam("@distid", distID);
                        dac.addParam("@rem", rem);
                        dac.addParam("@uid", uid);
                        dac.addParam("@iscomplete", completed);
                        dac.addParam("@sts", sts);
                        dr = dac.ExecuteReader(sql, CommandType.StoredProcedure);

                        oSerializer1 =
                        new System.Web.Script.Serialization.JavaScriptSerializer();
                        //sJSON = oSerializer1.Serialize(lstCall);
                        sJSON = "1";
                    }
                    break;
                case "otpgen":
                    amcdetailid = context.Request.QueryString["AMCDetailID"];
                    dac = new DataAccessS1();
                    dac.addParam("@amcdetailid", amcdetailid);
                    dr = dac.ExecuteReader("OTPgeneration", CommandType.StoredProcedure);
                    string otp = "", no = "";
                    while (dr.Read())
                    {
                        otp = dr["OTP"].ToString();
                        no = dr["Mobile1"].ToString();
                    }
                    dr.Close();
                    Api.Classes.Mailer.SendSMS(no, "OTP for call update is " + otp);
                    sJSON = "1";
                    break;
                case "amcalert":
                    uid = context.Request.QueryString["uid"];
                    sql = @"select P.Name [Product],AD.ProductSlNo,
                            convert(varchar(10),AD.startdate,103) StartDate,convert(varchar(10),AD.EndDate,103) EndDate,AD.AMCID
                                    from [dbo].[AMCDetails] AD join Master_AMC A on A.AMCID=AD.AMCID
                                    join [dbo].[Master_Party] MP on MP.PartyID=A.PartyID
									join [dbo].[Product] P on P.ProductID=AD.ProductID where A.PartyID=" + uid + " and GETDATE() > DATEADD(day,-15,EndDate)";

                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstCall = new List<CallList>();
                    while (dr.Read())
                    {
                        objCallList = new CallList();
                        objCallList.ProductSlNo = dr["ProductSlNo"].ToString();
                        objCallList.EndDate = dr["EndDate"].ToString();
                        objCallList.Product = dr["Product"].ToString();
                        lstCall.Add(objCallList);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstCall);
                    break;
                case "secallattended":

                    break;
                case "requestamc":
                    uid = context.Request.QueryString["uid"];
                    string partyname = context.Request.QueryString["partyname"];
                    sql = string.Format(@"INSERT INTO [dbo].[AmcRequests]
                                       ([PartyID]
                                       ,[ProductName]
                                       ,[SerialNo]
                                       ,[Model]
                                       ,[OtherInfo]
                                       )
                                        VALUES({0},'{1}','{2}','{3}','{4}')", uid, context.Request.QueryString["pname"], context.Request.QueryString["slno"],
                                                                             context.Request.QueryString["model"], context.Request.QueryString["other"]);
                    if (dac.executeTQuery(sql) > 0)
                    {
                        sJSON = "1";
                        string body = string.Format(@"Hello,< br /> New AMC request generated.Details below.<br/><strong>Party :&nbsp;</strong>{0}
                                                   <strong>Product :&nbsp;</strong>{1}<strong>Model :&nbsp;</strong>{2}<strong>Serial No :&nbsp;</strong>{3}
                                                   <strong>Remarks :&nbsp;</strong>{4}", partyname, context.Request.QueryString["pname"], context.Request.QueryString["model"]
                                                   , context.Request.QueryString["slno"], context.Request.QueryString["other"]);

                        Api.Classes.Mailer.SendMail(dac.GetAppSettings("AmcRequestMailID"), "New Amc Request", body);
                    }
                    break;
            }
            context.Response.Write(sJSON);
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
    public class CallList
    {
        public string ExpectedDate { get; set; }
        public string Party { get; set; }
        public string Address { get; set; }
        public string Mobile1 { get; set; }
        public string Product { get; set; }
        public string DistributionID { get; set; }
        public string Problem { get; set; }

        public string EndDate { get; set; }
        public string ProductSlNo { get; set; }
        public string AMCDetailID { get; set; }
        public string AMCID { get; set; }
    }

    public class Category
    {
        public string ID { get; set; }
        public string Cname { get; set; }
        public string CImg { get; set; }
    }
    public class Product
    {
        public string ID { get; set; }
        public string Pname { get; set; }
        public string PDesc { get; set; }
        public string PShortDesc { get; set; }
        public string Price { get; set; }
        //public string PDesc { get; set; }
        public string Pimg { get; set; }

        public string Qty { get; set; }
        public string PID { get; set; }
        public string OrderSts { get; set; }
        
    }
    public class Order
    {
        public string ID { get; set; }
        public string Pname { get; set; }
        public string PDesc { get; set; }
        public string PShortDesc { get; set; }
        public string Price { get; set; }
        //public string PDesc { get; set; }
        public string Pimg { get; set; }

        public string Qty { get; set; }
        public string PID { get; set; }
        public string OrderSts { get; set; }

    }
}