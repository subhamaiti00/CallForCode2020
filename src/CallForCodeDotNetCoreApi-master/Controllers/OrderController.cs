using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallForCodeApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CallForCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
       

        [HttpGet]
        [Route("ViewOrder")]
        public IActionResult ViewOrder(string uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=vieworder&uid=" + uid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("NewOrderAdmin")]
        public IActionResult NewOrderAdmin()
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=NewOrderAdmin";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("AssignedOrderAdmin")]
        public IActionResult AssignedOrderAdmin()
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=AssignedOrderAdmin";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("vieworderAdmin")]
        public IActionResult ViewOrderAdmin(string id)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=vieworderAdmin&id=" + id + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("Sehome")]
        public IActionResult Sehome(string uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=sehome&uid=" + uid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet]
        [Route("otpgen")]
        public IActionResult otpgen(string orderid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=otpgen&orderid="+ orderid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetSEForAssignmentDatewise")]
        public IActionResult GetSEForAssignmentDatewise(string fromdate, string todate)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=GetSEForAssignmentDatewise&fromdate=" + fromdate + "&todate=" + todate + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("AssignOrder")]
        public IActionResult AssignOrder(string fromdate, string todate, string IDs, string ID, string uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=AssignOrder&fromdate=" + fromdate + "&todate=" + todate + "&uid = " + uid + "&IDs = " + IDs + "&ID = " + ID + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [HttpPost]
        [Route("Order")]
        public IActionResult Order([FromBody] Order order)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=Order";
            string json = JsonConvert.SerializeObject(order);
            string html = RequestApi.PostApi(json, url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPost]
        [Route("updateorder")]
        public IActionResult Updaterder(string orderid, string otp, string rem, string uid, string sts)
        {
            try
            {
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=updateorder";
            Dictionary<string, string> obj = new Dictionary<string, string>();
            obj.Add("orderid", orderid);
            obj.Add("otp", otp);
            obj.Add("rem", rem);
            obj.Add("uid", uid);
            obj.Add("sts", sts);
            string json = JsonConvert.SerializeObject(obj);
            string html = RequestApi.PostApi(json, url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("OrderCompletionByVol")]
        public IActionResult OrderCompletionByVol(string uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=OrderCompletionByVol&uid="+ uid ;
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("OrderHistoryByDonor")]
        public IActionResult OrderHistoryByDonor(string uid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=OrderHistoryByDonor&uid=" + uid;
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
