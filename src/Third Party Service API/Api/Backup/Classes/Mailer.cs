using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;

namespace Api.Classes
{
    public static class Mailer
    {
        public static void SendMail(string to, string subject, string body)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("noreply@abahan.com", "Notification");
                Msg.To.Add(to);
                Msg.Body = body;
                Msg.Subject = subject;
                Msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();

                client.Host = "mail.abahan.com";
                client.Port = 25;
                client.Credentials = new NetworkCredential("noreply@abahan.com", "Subha123@");
                client.Send(Msg);
            }
            catch { }
        }

        public static int SendSMS(string contactNo, string msg)
        {
            WebResponse result = null;
            string output = "";
            int sendflag = 0;
            sendflag = 0;
            //string msg = "YOUR STUDENT ID IS " + formid + ", NETAJI NAGAR COLLEGE";
            msg = msg.Replace(' ', '+');
            //#######################################################
            //###   Please Change the API URL AS PER REQUIREMENT  ###
            //#######################################################---------v---------------v        
            //string API = "http://tranbulksms.infixia.in/api/sendmsg.php?user=demoinfixia&pass=demo123@&sender=INFIDM&phone=" + contactNo + "&text=" + msg + "&priority=ndnd&stype=normal";
            string API = "";// "https://fastsmsgateway.infixia.in/api/api_http.php?username=infixia1000&password=Dipti@Infi2018&senderid=INFIXI&to="+contactNo+"&text="+msg+"&route=Informative & type=text&datetime=2018-12-15%2013%3A01%3A52";
            API = "https://fastsmsgateway.infixia.in/api/api_http.php?username=infixia1000&password=Abir@2020Infi&senderid=INFIXI&to=" + contactNo + "&text=" + msg + "&route=Informative&type=text&datetime=2018-12-15%2013%3A01%3A52";

            try
            {
                WebRequest req = WebRequest.Create(API);
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, read.Length);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    output += str;
                    count = sr.Read(read, 0, read.Length);
                }
                sendflag = 1;
            }
            catch (Exception)
            {
                //Response.Write("get failed");
            }
            if (result != null)
            {
                result.Close();
            }
            return sendflag;
        }
    }
}