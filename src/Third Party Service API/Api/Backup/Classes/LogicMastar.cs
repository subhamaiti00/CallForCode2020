using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Asset.DAL
{
    /// <summary>
    /// Summary description for LogicMastar
    /// </summary>
    public class LogicMastar
    {
        public LogicMastar()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DateTime toDate(string ddmm)
        {
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";
            DateTime validDate = Convert.ToDateTime(ddmm, dateInfo);
            return validDate;
        }
        public string NumberToText(Int64 number)
        {
            if (number == 0) return "Zero";

            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

            Int64[] num = new Int64[4];
            Int64 first = 0;
            Int64 u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
"Five " ,"Six ", "Seven ", "Eight ", "Nine "};

            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
"Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};

            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
"Seventy ","Eighty ", "Ninety "};

            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }


            for (Int64 i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;

                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens
                try
                {
                    if (h > 0) sb.Append(words0[h] + "Hundred ");
                }
                catch
                {
                    h = num[i] / 1000;
                    t = t - 10 * h;
                    if (h > 0) sb.Append(words0[h] + "Thousand ");
                }

                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");

                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }

                if (i != 0) sb.Append(words3[i - 1]);

            }
            return sb.ToString().TrimEnd();
        }
        public string wholeText(decimal num)
        {
            string[] s = num.ToString().Split('.');
            string s1 = NumberToText(Int64.Parse(s[0].ToString()));
            if (int.Parse(s[1].ToString()) > 0)
            {
                string s2 = NumberToText(Int64.Parse(s[1].ToString()));
                return "Rupees " + s1 + " And Paise " + s2.Replace("And", string.Empty) + " Only";
            }
            else
            {
                return "Rupees " + s1 + " Only";
            }


        }
    }
}
