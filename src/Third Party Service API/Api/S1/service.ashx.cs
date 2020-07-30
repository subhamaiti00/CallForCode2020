using Archid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                        sJSON = "{\"uid\":\"" + sJSON.Split('~')[0] + "\",\"role\":\"" + sJSON.Split('~')[1].ToLower() + "\",\"name\":\"" + sJSON.Split('~')[2].ToLower() + "\",\"email\":\"" + sJSON.Split('~')[3] + "\",\"mob\":\"" + sJSON.Split('~')[4] + "\"}";
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
                    odem.Add("@UType", ss["utype"]);
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
                case "registerSE":
                    ss = oSerializer1.Deserialize<dynamic>(value);
                    odem = new DataManagerS1();
                    odem.Add("@UName", ss["name"]);
                    odem.Add("@Address", ss["add"]);
                    odem.Add("@Email", ss["uid"]);
                    odem.Add("@Pwd", ss["pwd"]);
                    odem.Add("@Mobile", ss["mob"]);
                    odem.Add("@UType", "s");
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
                    if (context.Request.QueryString["scatid"] == null)
                    {
                        sql = "select * from Products where IsDeleted=0";
                    }
                    else
                    {
                        sql="select * from Products where IsDeleted=0 and SubCategoryMasterID=" + context.Request.QueryString["scatid"];
                    }
                    SqlDataReader sqlDataReader3 = new DataAccessS1().ExecuteReader(sql, CommandType.Text);
                    List <Product> productList1 = new List<Product>();
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
                          ,O.[City]
                          ,O.[Pin]
                          ,O.[LandMark],Mobile
                      FROM [dbo].[OrderMaster] O join Users u on U.ID=O.CustID where O.Sts='OrderReceived'";
                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    List<Order> lstOrders = new List<Order>();
                    Order objOrder;
                    while (dr.Read())
                    {
                        objOrder = new Order();
                        objOrder.ID = dr["ID"].ToString();
                        objOrder.CustID = dr["CustID"].ToString();
                        objOrder.UName = dr["UName"].ToString();
                        objOrder.OrderValue = dr["OrderValue"].ToString();
                        objOrder.Date = dr["Date"].ToString();
                        objOrder.Remarks = dr["Remarks"].ToString();
                        objOrder.OrderNo = dr["OrderNo"].ToString();
                        objOrder.PayMode = dr["PayMode"].ToString();
                        objOrder.AdvAmt = dr["AdvAmt"].ToString();
                        objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
                        objOrder.PaymentID = dr["PaymentID"].ToString();
                        objOrder.Address = dr["Address"].ToString();
                        objOrder.City = dr["City"].ToString();
                        objOrder.Pin = dr["Pin"].ToString();
                        objOrder.LandMark = dr["LandMark"].ToString();
                        objOrder.Mobile = dr["Mobile"].ToString();
                        lstOrders.Add(objOrder);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstOrders);
                    break;
                case "AssignedOrderAdmin":
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
                          ,O.[City]
                          ,O.[Pin]
                          ,O.[LandMark],A.[SE_Names],CONVERT(varchar(10),A.[FromDate],103) [From]
						  ,CONVERT(varchar(10),A.[ToDate],103) [To]
                      FROM [dbo].[OrderMaster] O join Users u on U.ID=O.CustID 
					                             join AssignmentMaster A on A.OrderMasterID=O.ID
												  where O.Sts='Assigned' order by [FromDate] desc";
                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstOrders = new List<Order>();
                    while (dr.Read())
                    {
                        objOrder = new Order();
                        objOrder.ID = dr["ID"].ToString();
                        objOrder.CustID = dr["CustID"].ToString();
                        objOrder.UName = dr["UName"].ToString();
                        objOrder.OrderValue = dr["OrderValue"].ToString();
                        objOrder.Date = dr["Date"].ToString();
                        objOrder.Remarks = dr["Remarks"].ToString();
                        objOrder.OrderNo = dr["OrderNo"].ToString();
                        objOrder.PayMode = dr["PayMode"].ToString();
                        objOrder.AdvAmt = dr["AdvAmt"].ToString();
                        objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
                        objOrder.PaymentID = dr["PaymentID"].ToString();
                        objOrder.Address = dr["Address"].ToString();
                        objOrder.City = dr["City"].ToString();
                        objOrder.Pin = dr["Pin"].ToString();
                        objOrder.LandMark = dr["LandMark"].ToString();

                        objOrder.SE_Names = dr["SE_Names"].ToString();
                        objOrder.From = dr["From"].ToString();
                        objOrder.To = dr["To"].ToString();
                        lstOrders.Add(objOrder);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstOrders);
                    break;
                case "sehome":
                    uid = context.Request.QueryString["uid"];
                    sql = @"GetSEwiseJobs";
                    dac = new DataAccessS1();
                    dac.addParam("@seid", uid);
                    dr = dac.ExecuteReader(sql, CommandType.StoredProcedure);
                    lstOrders = new List<Order>();
                    while (dr.Read())
                    {
                        objOrder = new Order();
                        objOrder.ID = dr["ID"].ToString();
                        objOrder.CustID = dr["CustID"].ToString();
                        objOrder.UName = dr["UName"].ToString();
                        objOrder.OrderValue = dr["OrderValue"].ToString();
                        objOrder.Date = dr["Date"].ToString();
                        objOrder.Remarks = dr["Remarks"].ToString();
                        objOrder.OrderNo = dr["OrderNo"].ToString();
                        objOrder.PayMode = dr["PayMode"].ToString();
                        objOrder.AdvAmt = dr["AdvAmt"].ToString();
                        objOrder.AdvType_Half_Full = dr["AdvType_Half_Full"].ToString();
                        objOrder.PaymentID = dr["PaymentID"].ToString();
                        objOrder.Address = dr["Address"].ToString();
                        objOrder.City = dr["City"].ToString();
                        objOrder.Pin = dr["Pin"].ToString();
                        objOrder.LandMark = dr["LandMark"].ToString();
                        lstOrders.Add(objOrder);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstOrders);
                    break;
                case "vieworderAdmin":
                    dr = new DataAccessS1().ExecuteReader(@"select OM.ID,p.Pname,(OD.Price*OD.Qty) [Price],P.Pimg,OD.Qty,P.PShortDesc,P.ID [PID],OD.sts
                                                            from dbo.Products P join dbo.OrderDetails OD on P.ID=OD.ProductID join dbo.OrderMaster OM
                                                            on OD.OrderMasterID=OM.ID where P.IsDeleted=0 and OM.ID=" + context.Request.QueryString["id"], CommandType.Text);
                    productList4 = new List<Product>();
                    while (dr.Read())
                        productList4.Add(new Product()
                        {
                            ID = dr["ID"].ToString(),
                            Pname = dr["Pname"].ToString(),
                            Pimg = dr["Pimg"].ToString(),
                            PShortDesc = dr["PShortDesc"].ToString(),
                            Price = dr["Price"].ToString(),
                            Qty = dr["Qty"].ToString(),
                            PID = dr["PID"].ToString(),
                            OrderSts = dr["sts"].ToString()
                        });
                    dr.Close();
                    sJSON = new JavaScriptSerializer().Serialize((object)productList4);
                    break;
                case "GetSEForAssignmentDatewise":
                    //string fromdate = context.Request.QueryString["fromdate"].ToString().Replace("-", "");
                    //string todate = context.Request.QueryString["todate"].ToString().Replace("-", "");
                    //sql = "GetSEforOrderAssignment";
                    sql = "	select U.ID,U.UName from users u where U.UType='v'";
                    dac = new DataAccessS1();
                    //dac.addParam("@date", fromdate);
                    // dac.addParam("@enddate", todate);
                    dr = dac.ExecuteReader(sql, CommandType.Text);

                    List<SEList> lstSEList = new List<SEList>();
                    SEList objSEList;
                    while (dr.Read())
                    {
                        objSEList = new SEList();
                        objSEList.ID = dr["ID"].ToString();
                        objSEList.UName = dr["UName"].ToString();
                        objSEList.ischecked = false;
                        lstSEList.Add(objSEList);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstSEList);
                    break;
                case "AssignOrder":
                    string fromdate = context.Request.QueryString["fromdate"].ToString().Replace("-", "");
                    // todate = context.Request.QueryString["todate"].ToString().Replace("-", "");
                    string IDS = context.Request.QueryString["IDS"].ToString();
                    dac = new DataAccessS1();
                    dac.addParam("@fromdate", fromdate);
                    dac.addParam("@enddate", fromdate);
                    dac.addParam("@se_ids", IDS);
                    dac.addParam("@OrderMasterID", context.Request.QueryString["ID"].ToString());
                    dac.addParam("@AssignedBy", context.Request.QueryString["uid"].ToString());
                    if (dac.ExecuteQuerySP("SaveAssignOrder") > 0)
                    {
                        sJSON = "1";
                    }
                    break;
                case "otpgen":
                    string orderid = context.Request.QueryString["orderid"];
                    dac = new DataAccessS1();
                    dac.addParam("@orderid", orderid);
                    dr = dac.ExecuteReader("OTPgeneration", CommandType.StoredProcedure);
                    string otp = "", no = "";
                    while (dr.Read())
                    {
                        otp = dr["OTP"].ToString();
                        no = dr["Mobile"].ToString();
                    }
                    dr.Close();
                    Api.Classes.Mailer.SendSMS(no, "withU service completion OTP : " + otp);
                    sJSON = "1";
                    break;
                case "updateorder":
                    orderid = context.Request.QueryString["orderid"];
                    otp = context.Request.QueryString["otp"];
                    uid = context.Request.QueryString["uid"];
                    string rem = context.Request.QueryString["rem"];
                    string sts = context.Request.QueryString["sts"];
                    dac = new DataAccessS1();
                    dac.addParam("@orderid", orderid);
                    dac.addParam("@uid", uid);
                    dac.addParam("@rem", rem);
                    dac.addParam("@otp", otp);
                    dac.addParam("@sts", sts);
                    dr = dac.ExecuteReader("SpUpdateOrderSE", CommandType.StoredProcedure);
                    sJSON = "0";
                    while (dr.Read())
                    {
                        sJSON = dr["result"].ToString();
                        //no = dr["Mobile"].ToString();
                    }
                    dr.Close();

                    //sJSON = "1";
                    break;
                case "pendingverify":
                    sql = @"select ID,Uname,Address,dist,[state],Mobile,Pin,occupation from users 
                            where UType='c' and IsDeleted=0 and IsVerified=0 and
                            dist=(select dist from users where id=" + context.Request.QueryString["uid"] + ")";
                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    List<Consumers> lstCons = new List<Consumers>();
                    Consumers objCon;
                    sJSON = "0";
                    while (dr.Read())
                    {
                        objCon = new Consumers();
                        objCon.ID = dr["ID"].ToString();
                        objCon.UName = dr["Uname"].ToString();
                        objCon.Address = dr["Address"].ToString();
                        objCon.Dist = dr["dist"].ToString();
                        objCon.State = dr["state"].ToString();

                        objCon.Pin = dr["Pin"].ToString();
                        objCon.Mobile = dr["Mobile"].ToString();
                        objCon.occupation = dr["occupation"].ToString();

                        lstCons.Add(objCon);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstCons);
                    break;
                case "approve_reject":
                    string id = context.Request.QueryString["id"];
                    string action = context.Request.QueryString["action"];
                    if (action == "a")
                        sql = "update users set IsVerified=1 where Id=" + id;
                    else
                        sql = "update users set IsDeleted=1 where Id=" + id;
                    if (dac.executeTQuery(sql) > 0)
                    {
                        sJSON = "1";
                    }
                    break;
                case "OrderCompletionByVol":
                    sql = @"select O.ID,Uname,convert(varchar(10),O.Date,103) [Date],O.Sts,U.Dist,U.Address,U.Pin
                            from OrderMaster O join Users U on O.custid=U.ID where VolunteerID=" + context.Request.QueryString["uid"];
                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstOrders = new List<Order>();

                    while (dr.Read())
                    {
                        objOrder = new Order();
                        objOrder.ID = dr["ID"].ToString();
                        objOrder.UName = dr["UName"].ToString();
                        objOrder.Date = dr["Date"].ToString();
                        objOrder.Pin = dr["Pin"].ToString();
                        objOrder.City = dr["dist"].ToString();
                        objOrder.Address = dr["Address"].ToString();

                        lstOrders.Add(objOrder);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstOrders);
                    break;
                case "OrderHistoryByDonor":
                    uid = context.Request.QueryString["uid"];
                    sql = @"select O.ID,U.UName,CONVERT(varchar(10),O.Date,103) [Date],O.Sts from orderMaster O join [AssignmentMaster] A on A.OrderMasterID=O.ID
                            join Users U on O.CustID=U.ID  where SE_Names is not null and AssignedBy =" + uid;
                    dac = new DataAccessS1();
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstOrders = new List<Order>();

                    while (dr.Read())
                    {
                        objOrder = new Order();
                        objOrder.ID = dr["ID"].ToString();
                        objOrder.UName = dr["UName"].ToString();
                        objOrder.Date = dr["Date"].ToString();
                        objOrder.Sts = dr["Sts"].ToString();

                        lstOrders.Add(objOrder);
                    }
                    dr.Close();
                    oSerializer1 =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                    sJSON = oSerializer1.Serialize(lstOrders);
                    break;
                case "ProductRecom":
                    uid = context.Request.QueryString["uid"];
                    string[] product_cols = { "CUSTID", "Detergent Powder", "Dettol", "Geometry Sets", "Handwash", "Note Book", "Pen", "Pencil", "Rechargeble Solor Light", "Regular Rice 10 kg", "Solar Cooker", "Glucose solution", "Cerelac From 6 Months", "Cerelac From 12 Months", "Rice 5 kg", "Milk Cereal", "OZiva Protein", "Veg Soup", "Horlicks" };
                    DataTable df = ConvertCSVtoDataTable(context.Server.MapPath("~/s1/customers_orders1_opt_updated.csv"));
                    //DataTable df_filtered = df.Select(["CUST_ID"] + product_cols);
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
                        r["cluster"] = new Random().Next(10, 99);
                    }

                    dac = new DataAccessS1();
                    sql = @"select p.Pname,C.Qty from dbo.Products P join dbo.CartDetails C on P.ID=C.ProductID 
                            where P.IsDeleted=0 and C.CustID=" + uid;
                    dr = dac.ExecuteReader(sql, CommandType.Text);
                    lstProduct = new List<Product>();
                    //Product objProduct;
                    string fields = "", values = "";
                    string cols = "";
                    int cnt = 0;
                    while (dr.Read())
                    {

                        fields = fields + "\"sum(" + dr["Pname"].ToString() + ")\",";
                        cols += dr["Pname"].ToString() + ",";
                        values += Convert.ToInt16(dr["Qty"].ToString()).ToString() + ",";
                        cnt++;

                    }
                    dr.Close();
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
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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
                    var json = oSerializer1.Deserialize<dynamic>(responseString);
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
                    json = oSerializer1.Deserialize<dynamic>(responseString);
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
                    sqlDataReader3 = new DataAccessS1().ExecuteReader("select * from Products where IsDeleted=0", CommandType.Text);
                    productList1 = new List<Product>();
                    while (sqlDataReader3.Read())
                    {
                        foreach (DataColumn dc in df_cluster_products_agg.Columns)
                        {
                            if(sqlDataReader3["Pname"].ToString()== dc.ColumnName)
                            if (!string.IsNullOrEmpty(Convert.ToString(row_[dc.ColumnName])))
                            {
                                if (Convert.ToInt32(row_[dc.ColumnName].ToString())> 0)
                                    productList1.Add(new Product()
                                    {
                                        ID = sqlDataReader3["ID"].ToString(),
                                        Pname = sqlDataReader3["Pname"].ToString(),
                                        Pimg = sqlDataReader3["Pimg"].ToString(),
                                        PDesc = sqlDataReader3["PDesc"].ToString(),
                                        PShortDesc = sqlDataReader3["PShortDesc"].ToString(),
                                        Price = sqlDataReader3["Price"].ToString()
                                    });

                            }
                        }
                       
                    }
                    sqlDataReader3.Close();                    
                   
                    sJSON = new JavaScriptSerializer().Serialize((object)productList1);
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
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
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
        public string CustID { get; set; }
        public string UName { get; set; }
        public string OrderValue { get; set; }
        public string Date { get; set; }
        public string Sts { get; set; }
        public string Remarks { get; set; }

        public string OrderNo { get; set; }
        public string PayMode { get; set; }
        public string AdvAmt { get; set; }
        public string AdvType_Half_Full { get; set; }
        public string PaymentID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public string Mobile { get; set; }

        public string SE_Names { get; set; }
        public string From { get; set; }
        public string To { get; set; }

    }

    public class SEList
    {
        public string ID { get; set; }
        public string UName { get; set; }
        public bool ischecked { get; set; }
    }

    public class Consumers
    {
        public string ID { get; set; }
        public string UName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Dist { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string occupation { get; set; }
        

    }
}