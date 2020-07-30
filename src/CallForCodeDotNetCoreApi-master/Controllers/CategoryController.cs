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
    public class CategoryController : ControllerBase
    {
        //private readonly DatabaseContext _database;
        //public CategoryController(DatabaseContext context)
        //{
        //    _database = context;
        //}

        [HttpGet]
        [Route("Category")]
        public IActionResult Category()
        {
            try { 

            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=Categories";
            string html = RequestApi.GetApi(url);
            return Ok(html);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("SubCategory")]
        public IActionResult SubCategory(int catid)
        {
            try { 
            string url = @"http://callforcodeapi.abahan.com//S1/service.ashx?method=SubCategories&catid='" + catid + "'";
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
