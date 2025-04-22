using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] //GET: /api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            //Code 
            return NotFound();
        }

        [HttpGet("servererror")] //GET: /api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok();
        }

        [HttpGet(template: "badrequest/{id}")]
        public IActionResult GetBadRequest(int id) // GET: /api/Badgy/badrequest/ahmed  
        {
            // Code  
            return BadRequest(); // 400  
        }

        [HttpGet(template: "unauthorized")] // GET: /api/Buggy/unauthorized  
        public IActionResult GetUnauthorizedRequest()
        {
            // Code  
            return Unauthorized(); // 401
        }


    }
}
