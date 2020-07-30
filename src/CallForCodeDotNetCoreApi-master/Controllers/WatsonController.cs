using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallForCodeApi.Model;
using IBM.Watson.Assistant.v2;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallForCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatsonController : ControllerBase
    {        
        private readonly DatabaseContext _database;
        public WatsonController(DatabaseContext context)
        {
            _database = context;
        }

        [HttpGet("{request}")]
        public string GetWatsonAssistantResponse(string request)
        {
            string genericText = string.Empty;
            try
            {
                _database.CreateSession();
                genericText = _database.Message(request);               
                _database.DeleteSession();
            }
            catch (Exception ex)
            {
               return ex.Message.ToString();
            }           
            return genericText;
        }        
    }
}
