using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallForCodeApi.Model
{
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
}
