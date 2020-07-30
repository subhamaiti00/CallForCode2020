using CallForCodeApi.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CallForCodeApi.Controllers
{
    public class CartController : ControllerBase
    {
        [HttpGet]
        [Route("AddToCart")]
        public IActionResult AddToCart(int uid, int pid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=addtocart&uid=" + uid + "&pid=" + pid + "";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("ViewCart")]
        public IActionResult ViewCart(int uid)
        {
            try
            {
                string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=viewcart&uid='" + uid + "'";
                string html = RequestApi.GetApi(url);
                return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("RemoveCart")]
        public IActionResult RemoveCart(string cartid)
        {
            try
            {
                string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=removefromcart&cartid='" + cartid + "'";
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
