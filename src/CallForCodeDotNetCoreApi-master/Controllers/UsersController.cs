using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CallForCodeApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CallForCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string uid, string pwd)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=login";
            Dictionary<string, string> obj = new Dictionary<string, string>();
            obj.Add("uid", uid);
            obj.Add("pwd", pwd);
            string json = JsonConvert.SerializeObject(obj);
            string html = RequestApi.PostApi(json,url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] Users user)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=register";
            string json = JsonConvert.SerializeObject(user);
            string html = RequestApi.PostApi(json, url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("RegisterSE")]
        public IActionResult registerSE([FromBody] Users user)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=registerSE";
            string json = JsonConvert.SerializeObject(user);
            string html = RequestApi.PostApi(json, url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("pendingverify")]
        public IActionResult pendingverify()
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=pendingverify";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("approve_reject")]
        public IActionResult approve_reject(int id)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=approve_reject&id=" + id;
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
