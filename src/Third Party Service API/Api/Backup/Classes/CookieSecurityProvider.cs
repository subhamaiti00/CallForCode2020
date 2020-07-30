using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;


    public static class CookieSecurityProvider
    {
        // private static MethodInfo _encode;
        // private static MethodInfo _decode;
        // // CookieProtection.All enables 'temper proffing' and 'encryption' for cookie
        // private static CookieProtection _cookieProtection = CookieProtection.All;

        // //Static constructor to get reference of Encode and Decode methods of Class CookieProtectionHelper
        // //using Reflection.
        // static CookieSecurityProvider()
        // {
        //     Assembly systemWeb = typeof(HttpContext).Assembly;
        //     Type cookieProtectionHelper = systemWeb.GetType("System.Web.Security.CookieProtectionHelper");
        //     _encode = cookieProtectionHelper.GetMethod("Encode", BindingFlags.NonPublic | BindingFlags.Static);
        //     _decode = cookieProtectionHelper.GetMethod("Decode", BindingFlags.NonPublic | BindingFlags.Static);
        // }

        // public static HttpCookie Encrypt(HttpCookie httpCookie)
        // {
        //     byte[] buffer = Encoding.Default.GetBytes(httpCookie.Value);

        //     //Referencing the Encode mehod of CookieProtectionHelper class
        //     httpCookie.Value = (string)_encode.Invoke(null, new object[] { _cookieProtection, buffer, buffer.Length });
        //     return httpCookie;
        // } 

        // public static HttpCookie Decrypt(HttpCookie httpCookie)
        // {
        //     //Referencing the Decode mehod of CookieProtectionHelper class
        //     byte[] buffer = (byte[])_decode.Invoke(null, new object[] { _cookieProtection, httpCookie.Value });
        //     httpCookie.Value = Encoding.Default.GetString(buffer, 0, buffer.Length);
        //     return httpCookie;
        // }
        // public static string EncryptStr(string str)
        // {
        //     byte[] buffer = Encoding.Default.GetBytes(str);
        //     return (string)_encode.Invoke(null, new object[] { _cookieProtection, buffer, buffer.Length });
        // }
        // public static string DecryptStr(string str)
        // {
        //     byte[] buffer = (byte[])_decode.Invoke(null, new object[] { _cookieProtection, str });
        //     return Encoding.Default.GetString(buffer, 0, buffer.Length);
        // }


        public static HttpCookie Encrypt(HttpCookie httpCookie)
        {

            httpCookie.Value = MockTest.Security.EncryptText(httpCookie.Value);
            return httpCookie;
        }

        public static HttpCookie Decrypt(HttpCookie httpCookie)
        {
            //Referencing the Decode mehod of CookieProtectionHelper class

            httpCookie.Value =MockTest.Security.DecryptText(httpCookie.Value);
            return httpCookie;
        }

        public static string EncryptStr(string s)
        {
            return MockTest.Security.EncryptText(s);
        }
        public static string DecryptStr(string s)
        {
            return MockTest.Security.DecryptText(s);
        }
    }

