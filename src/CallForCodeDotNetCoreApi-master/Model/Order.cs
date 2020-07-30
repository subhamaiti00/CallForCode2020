using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallForCodeApi.Model
{
    public class Order
    {
        public string uid { get; set; }
        public string pid { get; set; }
        public string payAmt { get; set; }
        public string ActualAmt { get; set; }
        public string AdvAmt { get; set; }
        public string PaymentMode { get; set; }
        public string AdvType_Half_Full { get; set; }
        public string Qty { get; set; }
        public string paymentID { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string pin { get; set; }
        public string landmark { get; set; }

    }
}
