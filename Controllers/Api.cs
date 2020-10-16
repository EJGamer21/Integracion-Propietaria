using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Iso810.Entities;

namespace Iso810.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly MainContext _context;

        public ApiController(MainContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IEnumerable<StudentsView>> Get()
        {
            return await _context.StudentsView.ToListAsync();
        }

        [HttpPost("upload")]
        public async Task<IEnumerable<StudentsView>> Upload([FromBody] int id)
        {             

            return await _context.StudentsView.ToListAsync();
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            try
            {
                var response = await _context.StudentsView.ToListAsync();
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been detected.", ex);
            }
        }
    }
}